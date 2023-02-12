Shader"Unlit/GearEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("NormalMap", 2D) = "bump"{}
        //[NoScaleOffset] _MetallicMap ("MetallicMap", 2D) = "white" {}
        //[NoScaleOffset] _OcculusionMap ("OcculusionMap", 2D) = "white"{}
        //[NoScaleOffset] _EmissionMap ("EmissionMap", 2D) = "white" {}
        _DissolveTex ("DissolveTexture", 2D) = "white" {}
        _DissolveAmount ("DissolveAmount", Range(0, 1)) = 0
        _DissolveRamge ("DissolveRange", Range(0, 1)) = 0
        [HDR]_DissolveColor ("DissolveColor",Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        Blend One OneMinusSrcAlpha
        LOD 100

        Pass
        {
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
                half4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 normal : TEXCOORD1;
                half3 tangent : TEXCOORD2;
                half3 binormal : TEXCOORD3;
                float4 worldPos : TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NormalMap;
            float4 _NormalMap_ST;

            sampler2D _MetallicMap;
            float4 _MetallicMap_ST;

            sampler2D _OcculusionMap;
            float4 _OcculusionMap_ST;

            sampler2D _EmissionMap;
            float4 _EmissionMap_ST;

            float3 _AmbientLight;

            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            float _DissolveAmount;
            float _DissolveRamge;
            float4 _DissolveColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.tangent = normalize(TransformObjectToWorldDir(v.tangent.xyz));
                o.binormal = cross(o.normal, o.tangent.xyz) * v.tangent.w;
                o.binormal = normalize(TransformObjectToWorldDir(o.binormal));
                o.worldPos = v.vertex;
    
                return o;
            }

            float remap (float value, float outMin, float outMax)
{
    return value * ((outMax - outMin) / 1) + outMin;
}

            half4 frag (v2f i) : SV_Target
            {
                half3 normalmap = UnpackNormal(tex2D(_NormalMap, i.uv));
    
                // ‰AŠÖŒW
                float3 normal = (i.tangent * normalmap.x) + (i.binormal * normalmap.y) + (i.normal * normalmap.z);
                normal = normalize(normal);
                Light light = GetMainLight();
                float diffuse = dot(normal, light.direction);
                half4 col = tex2D(_MainTex, i.uv);
                col.rgb *= diffuse;
    
                // dissolve
                float dissolve = tex2D(_DissolveTex, i.uv).r;
                _DissolveAmount = remap(_DissolveAmount, -_DissolveRamge, 1);
                if (dissolve < _DissolveAmount + _DissolveRamge)
                {
                    col += _DissolveColor;
        
                    if (dissolve < _DissolveAmount)
                    {
                        col.a = 0;
                    }
                }
                
                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}
