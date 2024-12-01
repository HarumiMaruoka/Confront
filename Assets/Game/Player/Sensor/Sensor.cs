using Confront.StageGimmick;
using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "Sensor", menuName = "Confront/Player/Sensor", order = 1)]
    public class Sensor : ScriptableObject
    {
        [Header("Check Ground")]
        [SerializeField]
        private Vector2 _groundCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _groundCheckRayRadius = 0f;
        [SerializeField]
        private float _groundCheckRayLength = 0f;
        [SerializeField]
        private bool _isGroundCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isGroundCheckGizmoAlpha = 0.5f;

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

        [Header("Layer Mask")]
        [SerializeField]
        private LayerMask _groundLayerMask;
        [SerializeField]
        private LayerMask _passThroughPlatform;
        [SerializeField]
        private LayerMask _grabbablePointLayerMask = 0;

        public SensorResult Calculate(PlayerController player)
        {
            var result = new SensorResult();

            var groundCheckRayPosition = player.transform.position + (Vector3)_groundCheckRayOffset;
            var abyssCheckRayPosition = player.transform.position + (Vector3)_abyssCheckRayOffset;
            var slopeLimit = player.CharacterController.slopeLimit;

            LayerMask groundCheckLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished) 
                groundCheckLayerMask = _groundLayerMask | _passThroughPlatform;
            else 
                groundCheckLayerMask = _groundLayerMask;

            // 足元にレイを飛ばして、ヒットした場合には、地面にいると判定する。
            result.IsGrounded = UnityEngine.Physics.SphereCast(groundCheckRayPosition, _groundCheckRayRadius, Vector3.down, out var groundHit, _groundCheckRayLength, groundCheckLayerMask);
            // 足元に小さなレイを飛ばして、ヒットしなかった場合には、崖にいると判定する。
            result.IsAbyss = !UnityEngine.Physics.SphereCast(abyssCheckRayPosition, _abyssCheckRayRadius, Vector3.down, out var abyssHit, _abyssCheckRayLength, groundCheckLayerMask);

            if (result.IsGrounded)
            {
                result.GroundNormal = groundHit.normal;
                result.IsSteepSlope = Vector3.Angle(Vector3.up, groundHit.normal) > slopeLimit;
            }

            if (result.IsGrounded && result.IsAbyss)
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
            result.IsAbove = UnityEngine.Physics.Raycast(player.transform.position + (Vector3)_aboveCheckRayOffset, Vector3.up, _aboveCheckRayLength, _groundLayerMask);

            // 掴めるポイントを探す
            var position = player.transform.position + player.transform.rotation * (Vector3)_grabbablePointOffset;
            UnityEngine.Physics.SphereCast(position, _grabbablePointRadius, player.transform.rotation * Vector3.forward, out var grabPointHit, _grabbablePointLength, _grabbablePointLayerMask);
            if (grabPointHit.transform) result.GrabbablePoint = grabPointHit.transform.GetComponent<GrabbablePoint>();

            return result;
        }

        public void DrawGizmos(PlayerController player)
        {
            var result = Calculate(player);

            if (_isGroundCheckGizmoEnabled)
            {
                var groundCheckRayPosition = player.transform.position + (Vector3)_groundCheckRayOffset;
                var groundCheckRayEnd = groundCheckRayPosition + Vector3.down * _groundCheckRayLength;

                Gizmos.color = result.IsGrounded ? new Color(1, 0, 0, _isGroundCheckGizmoAlpha) : new Color(0, 1, 0, _isGroundCheckGizmoAlpha);

                Gizmos.DrawWireSphere(groundCheckRayPosition, _groundCheckRayRadius);
                Gizmos.DrawWireSphere(groundCheckRayEnd, _groundCheckRayRadius);

                Vector3 leftOffset = Vector3.left * _groundCheckRayRadius;
                Vector3 rightOffset = Vector3.right * _groundCheckRayRadius;
                Gizmos.DrawLine(groundCheckRayPosition + leftOffset, groundCheckRayEnd + leftOffset);
                Gizmos.DrawLine(groundCheckRayPosition + rightOffset, groundCheckRayEnd + rightOffset);
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
        }
    }

    public struct SensorResult
    {
        public bool IsGrounded; // 地面と接地しているか
        public bool IsAbyss; // 崖にいるか
        public bool IsSteepSlope; // 急斜面にいるか
        public bool IsAbove; // 頭上に何かあるか

        public Vector2 GroundNormal; // 地面の法線

        public GroundType GroundType;

        public GrabbablePoint GrabbablePoint; // 掴めるポイント
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