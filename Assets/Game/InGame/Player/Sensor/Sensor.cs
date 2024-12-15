using Confront.StageGimmick;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "Sensor", menuName = "ConfrontSO/Player/Sensor", order = 1)]
    public class Sensor : ScriptableObject
    {
        [Header("Check Ground")]
        [SerializeField]
        public Vector2 _groundCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float angleRange = 180f;
        [SerializeField]
        private int rayCount = 32;
        [SerializeField]
        public float rayDistance = 1;
        [SerializeField]
        private bool _isGroundCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isGroundCheckGizmoAlpha = 0.5f;

        private List<RayInfo> rayInfos = new List<RayInfo>();

        [Header("Check Abyss")]
        [SerializeField]
        private Vector2 _abyssCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _abyssCheckRayRadius = 0f;
        [SerializeField]
        private float _abyssCheckRayLength = 0f;
        [SerializeField]
        private bool _isAbyssCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isAbyssCheckGizmoAlpha = 0.5f;

        [Header("Check Above")]
        [SerializeField]
        private Vector2 _aboveCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _aboveCheckRayLength = 0f;
        [SerializeField]
        private bool _isAboveCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isAboveCheckGizmoAlpha = 0.5f;

        [Header("Check Grabbable Point")]
        [SerializeField]
        private Vector3 _grabbablePointOffset = new Vector3(0, 2.21f);
        [SerializeField]
        private float _grabbablePointRadius = 0f;
        [SerializeField]
        private float _grabbablePointLength = 0f;
        [SerializeField]
        private bool _isGrabbablePointCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isGrabPointCheckGizmoAlpha = 0.5f;

        [Header("Check Front")]
        [SerializeField]
        private Vector2 _frontCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _frontCheckRayLength = 0f;
        [SerializeField]
        private bool _isFrontCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isFrontCheckGizmoAlpha = 0.5f;

        [Header("Layer Mask")]
        public LayerMask GroundLayerMask;
        public LayerMask PassThroughPlatform;
        public LayerMask GrabbablePointLayerMask;

        public bool FrontCheck(PlayerController player, float sign)
        {
            var frontCheckRayPosition = player.transform.position + (Vector3)_frontCheckRayOffset;
            return UnityEngine.Physics.Raycast(frontCheckRayPosition, Vector3.right * sign, _frontCheckRayLength, GroundLayerMask);
        }

        private int _prevFrameCount = -1;
        private SensorResult _prevResult;

        public SensorResult Calculate(PlayerController player)
        {
            //if (_prevFrameCount == Time.frameCount)
            //{
            //    return _prevResult;
            //}

            var result = new SensorResult();

            var groundCheckRayPosition = player.transform.position + (Vector3)_groundCheckRayOffset;
            var abyssCheckRayPosition = player.transform.position + (Vector3)_abyssCheckRayOffset;
            var slopeLimit = player.CharacterController.slopeLimit;

            LayerMask groundCheckLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished)
                groundCheckLayerMask = GroundLayerMask | PassThroughPlatform;
            else
                groundCheckLayerMask = GroundLayerMask;

            // 足元にレイを飛ばして、ヒットした場合には、地面にいると判定する。
            var groundNormal = CastFanRaysAndGetAveragedNormal(player, Vector3.down, out float averageHitRayLength);
            result.AverageHitRayLength = averageHitRayLength;
            // 足元に小さなレイを飛ばして、ヒットしなかった場合には、崖にいると判定する。
            result.IsAbyss = !UnityEngine.Physics.SphereCast(abyssCheckRayPosition, _abyssCheckRayRadius, Vector3.down, out var abyssHit, _abyssCheckRayLength, groundCheckLayerMask);

            var groundAngle = Vector3.Angle(Vector3.up, groundNormal);
            result.IsGrounded = groundNormal != Vector3.zero && groundAngle < 89.999f;

            if (result.IsGrounded)
            {
                result.GroundNormal = groundNormal;
                result.GroundPoint = groundCheckRayPosition + (Vector3)groundNormal * averageHitRayLength;
                result.IsSteepSlope = groundAngle < 89.999f && Vector3.Angle(Vector3.up, groundNormal) > slopeLimit;
            }

            if (result.IsGrounded && result.IsAbyss && groundAngle < 89.999f)
            {
                result.GroundType = GroundType.Abyss;
            }
            else if (result.IsSteepSlope)
            {
                result.GroundType = GroundType.SteepSlope;
            }
            else if (result.IsGrounded && !result.IsAbyss)
            {
                result.GroundType = GroundType.Ground;
            }
            else
            {
                result.GroundType = GroundType.InAir;
            }

            // 頭上にレイを飛ばして、ヒットした場合には、頭上に何かあると判定する。
            result.IsAbove = UnityEngine.Physics.Raycast(player.transform.position + (Vector3)_aboveCheckRayOffset, Vector3.up, _aboveCheckRayLength, GroundLayerMask);

            // 掴めるポイントを探す
            var position = player.transform.position + player.transform.rotation * (Vector3)_grabbablePointOffset;
            UnityEngine.Physics.SphereCast(position, _grabbablePointRadius, player.transform.rotation * Vector3.forward, out var grabPointHit, _grabbablePointLength, GrabbablePointLayerMask);
            if (grabPointHit.transform) result.GrabbablePoint = grabPointHit.transform.GetComponent<GrabbablePoint>();

            _prevFrameCount = Time.frameCount;
            _prevResult = result;

            return result;
        }

        public void DrawGizmos(PlayerController player)
        {
            var result = Calculate(player);

            if (_isGroundCheckGizmoEnabled && rayInfos != null)
            {
                foreach (var rayInfo in rayInfos)
                {
                    Gizmos.color = rayInfo.hit ? new Color(1, 0, 0, _isGroundCheckGizmoAlpha) : new Color(0, 0, 1, _isGroundCheckGizmoAlpha);
                    Gizmos.DrawLine(rayInfo.start, rayInfo.end);
                }
            }

            if (_isAbyssCheckGizmoEnabled)
            {
                var abyssCheckRayPosition = player.transform.position + (Vector3)_abyssCheckRayOffset;
                var abyssCheckRayEnd = abyssCheckRayPosition + Vector3.down * _abyssCheckRayLength;

                Gizmos.color = result.IsAbyss ? new Color(1, 0, 0, _isAbyssCheckGizmoAlpha) : new Color(0, 1, 0, _isAbyssCheckGizmoAlpha);

                Gizmos.DrawWireSphere(abyssCheckRayPosition, _abyssCheckRayRadius);
                Gizmos.DrawWireSphere(abyssCheckRayEnd, _abyssCheckRayRadius);

                Vector3 leftOffset = Vector3.left * _abyssCheckRayRadius;
                Vector3 rightOffset = Vector3.right * _abyssCheckRayRadius;
                Gizmos.DrawLine(abyssCheckRayPosition + leftOffset, abyssCheckRayEnd + leftOffset);
                Gizmos.DrawLine(abyssCheckRayPosition + rightOffset, abyssCheckRayEnd + rightOffset);
            }

            if (_isAboveCheckGizmoEnabled)
            {
                var aboveCheckRayPosition = player.transform.position + (Vector3)_aboveCheckRayOffset;
                var aboveCheckRayEnd = aboveCheckRayPosition + Vector3.up * _aboveCheckRayLength;

                Gizmos.color = result.IsAbove ? new Color(1, 0, 0, _isAboveCheckGizmoAlpha) : new Color(0, 1, 0, _isAboveCheckGizmoAlpha);

                Gizmos.DrawLine(aboveCheckRayPosition, aboveCheckRayEnd);
            }

            if (_isGrabbablePointCheckGizmoEnabled)
            {
                var position = player.transform.position + player.transform.rotation * (Vector3)_grabbablePointOffset;
                var grabPointEnd = position + player.transform.rotation * Vector3.forward * _grabbablePointLength;
                Gizmos.color = result.GrabbablePoint ? new Color(1, 0, 0, _isGrabPointCheckGizmoAlpha) : new Color(0, 1, 0, _isGrabPointCheckGizmoAlpha);
                Gizmos.DrawWireSphere(position, _grabbablePointRadius);
                Gizmos.DrawWireSphere(grabPointEnd, _grabbablePointRadius);
                Vector3 upOffset = Vector3.up * _grabbablePointRadius;
                Vector3 downOffset = Vector3.down * _grabbablePointRadius;
                Gizmos.DrawLine(position + upOffset, grabPointEnd + upOffset);
                Gizmos.DrawLine(position + downOffset, grabPointEnd + downOffset);
            }

            if (_isFrontCheckGizmoEnabled)
            {
                var frontCheckRayPosition = player.transform.position + (Vector3)_frontCheckRayOffset;
                var frontCheckRayEnd = frontCheckRayPosition + player.transform.forward * _frontCheckRayLength;
                var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                var isFront = FrontCheck(player, sign);
                Gizmos.color = isFront ? new Color(1, 0, 0, _isFrontCheckGizmoAlpha) : new Color(0, 1, 0, _isFrontCheckGizmoAlpha);
                Gizmos.DrawLine(frontCheckRayPosition, frontCheckRayEnd);
            }
        }

        private Vector3 CastFanRaysAndGetAveragedNormal(PlayerController player, Vector3 direction, out float averageHitRayLength)
        {
            var origin = player.transform.position + (Vector3)_groundCheckRayOffset;
            LayerMask groundCheckLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished)
                groundCheckLayerMask = GroundLayerMask | PassThroughPlatform;
            else
                groundCheckLayerMask = GroundLayerMask;

            List<Vector3> hitNormals = new List<Vector3>();
            rayInfos.Clear(); // 前回のRay情報をクリア
            direction = direction.normalized;

            float halfAngle = angleRange * 0.5f;

            averageHitRayLength = 0f;
            for (int i = 0; i < rayCount; i++)
            {
                float t = (float)i / (rayCount - 1);
                float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

                Quaternion rot = Quaternion.Euler(0, 0, angle);
                Vector3 dir = rot * direction;

                Ray ray = new Ray(origin, dir);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit, rayDistance, groundCheckLayerMask);

                if (isHit)
                {
                    hitNormals.Add(hit.normal);
                    rayInfos.Add(new RayInfo { start = origin, end = hit.point, hit = true });
                    averageHitRayLength += hit.distance;
                }
                else
                {
                    rayInfos.Add(new RayInfo { start = origin, end = origin + dir * rayDistance, hit = false });
                }
            }

            if (hitNormals.Count == 0)
            {
                averageHitRayLength = rayDistance;
                return Vector3.zero;
            }

            Vector3 sum = Vector3.zero;
            foreach (var normal in hitNormals)
            {
                sum += normal;
            }
            Vector3 averageNormal = sum / hitNormals.Count;
            averageHitRayLength /= hitNormals.Count;
            averageNormal.Normalize();

            return averageNormal;
        }

        private struct RayInfo
        {
            public Vector3 start;
            public Vector3 end;
            public bool hit;
        }
    }

    public struct SensorResult
    {
        public bool IsGrounded; // 地面と接地しているか
        public bool IsAbyss; // 崖にいるか
        public bool IsSteepSlope; // 急斜面にいるか
        public bool IsAbove; // 頭上に何かあるか
        public float AverageHitRayLength; // 平均のヒットしたレイの長さ

        public Vector2 GroundNormal; // 地面の法線
        internal Vector2 GroundPoint;

        public GroundType GroundType;

        public GrabbablePoint GrabbablePoint; // 掴めるポイント

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(SensorResult a, SensorResult b)
        {
            return a.IsGrounded == b.IsGrounded &&
                   a.IsAbyss == b.IsAbyss &&
                   a.IsSteepSlope == b.IsSteepSlope &&
                   a.IsAbove == b.IsAbove &&
                   a.GroundNormal == b.GroundNormal &&
                   a.GroundType == b.GroundType &&
                   a.GrabbablePoint == b.GrabbablePoint;
        }

        public static bool operator !=(SensorResult a, SensorResult b)
        {
            return !(a == b);
        }
    }

    public enum GroundType
    {
        None,
        Ground,
        Abyss,
        SteepSlope,
        InAir
    }
}