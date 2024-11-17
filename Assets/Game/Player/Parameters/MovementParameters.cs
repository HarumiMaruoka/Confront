using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "MovementParameters", menuName = "Confront/Player/Movement Parameters")]
    public class MovementParameters : ScriptableObject
    {
        private static readonly string _addressablePath = "MovementParameters";
        private static MovementParameters _instance;

        public static MovementParameters Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var handle = Addressables.LoadAssetAsync<MovementParameters>(_addressablePath);
                handle.WaitForCompletion();
                return _instance = handle.Result;
            }
        }

        [HideInInspector]
        public Vector2 Velocity;

        [Header("Jump")]
        public float JumpForce;

        [Header("Grab")]
        public float GrabInterval;
        public Vector2 GrabPositionOffset;
        public float GrabDuration;
        public Vector2 ClimbPositionOffset;
        public AnimationCurve ClimbUpFlow; // 上昇アニメーション
        public AnimationCurve TraverseFlow; // 横移動アニメーション
        public float ClimbToRunThreshold;

        [HideInInspector]
        public Vector3 GrabbablePosition;
        [HideInInspector]
        public float GrabIntervalTimer = -1;

        public bool IsGrabbableTimerFinished => GrabIntervalTimer <= 0;

        [Header("Grounded")]
        public float Acceleration;
        public float Deceleration;
        public float TurnDeceleration;
        public float MaxSpeed;

        [Header("In Air")]
        public float Gravity;
        public float InAirXAcceleration;
        public float InAirXDeceleration;
        public float InAirXMaxSpeed;

        [Header("Abyss")]
        public float AbyssAcceleration;
        public float AbyssXMinSpeed;

        [Header("Steep Slope")]
        public float SlopeAcceleration;
    }
}