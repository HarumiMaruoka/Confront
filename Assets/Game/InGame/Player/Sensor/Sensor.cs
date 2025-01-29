using Confront.StageGimmick;
using Confront.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "Sensor", menuName = "ConfrontSO/Player/Sensor", order = 1)]
    public class Sensor : ScriptableObject
    {
        [Header("Check Ground")]
        [SerializeField]
        public Vector2 _groundCheckRayOffset1 = new Vector2(0, 0.1f);
        [SerializeField]
        public float _groundCheckRayLength = 0.58f;
        [SerializeField]
        public Vector2 _groundCheckRayOffset2 = new Vector2(0, 0.1f);
        [SerializeField]
        private float _groundCheckRayRadius = 0.3f;
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

        [Header("Check Front")]
        [SerializeField]
        private Vector2 _frontCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _frontCheckRayLength = 0f;
        [SerializeField]
        private Vector3 _frontCheckBoxRayHalfSize = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField]
        private bool _isFrontCheckGizmoEnabled = true;
        [SerializeField]
        [Range(0.3f, 1f)]
        private float _isFrontCheckGizmoAlpha = 0.5f;

        [Header("Layer Mask")]
        public LayerMask GroundLayerMask;
        public LayerMask EnemyLayerMask;
        public LayerMask PassThroughPlatform;
        public LayerMask GrabbablePointLayerMask;

        public bool FrontCheck(PlayerController player, float sign)
        {
            LayerMask groundCheckLayerMask = GroundLayerMask | EnemyLayerMask;

            var frontCheckRayPosition = player.transform.position + (Vector3)_frontCheckRayOffset;
            return UnityEngine.Physics.BoxCast(
                frontCheckRayPosition,
                _frontCheckBoxRayHalfSize,
                Vector3.right * sign,
                Quaternion.identity,
                _frontCheckRayLength,
                groundCheckLayerMask,
                QueryTriggerInteraction.Ignore);
        }

        public bool IsAbove(PlayerController player)
        {
            // 頭上にレイを飛ばして、ヒットした場合には、頭上に何かあると判定する。
            return UnityEngine.Physics.Raycast(player.transform.position + (Vector3)_aboveCheckRayOffset, Vector3.up, _aboveCheckRayLength, GroundLayerMask);
        }

        public GrabbablePoint GetGrabbablePoint(PlayerController player)
        {
            // 掴めるポイントを探す
            var position = player.transform.position + player.transform.rotation * (Vector3)_grabbablePointOffset;
            UnityEngine.Physics.SphereCast(position, _grabbablePointRadius, player.transform.rotation * Vector3.forward, out var grabPointHit, _grabbablePointLength, GrabbablePointLayerMask);
            if (grabPointHit.transform) return grabPointHit.transform.GetComponent<GrabbablePoint>();
            return null;
        }

        public bool IsGroundBelow(PlayerController player, LayerMask layerMask)
        {
            var abyssCheckRayOrigin = player.transform.position + (Vector3)_abyssCheckRayOffset;
            return UnityEngine.Physics.SphereCast(abyssCheckRayOrigin, _abyssCheckRayRadius, Vector3.down, out var hit, _abyssCheckRayLength, layerMask);
        }

        private int _steepSlopeCount = 0;

        public GroundSensorResult CalculateGroundState(PlayerController player)
        {
            var result = new GroundSensorResult();

            LayerMask groundCheckLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished)
                groundCheckLayerMask = GroundLayerMask | EnemyLayerMask | PassThroughPlatform;
            else
                groundCheckLayerMask = GroundLayerMask | EnemyLayerMask;

            // 足元にレイを飛ばして、ヒットした場合には、地面にいると判定する。
            Vector3 groundCheckRayOrigin = player.transform.position + (Vector3)_groundCheckRayOffset1;
            var isHitSphereCast = Physics.SphereCast(groundCheckRayOrigin, _groundCheckRayRadius, Vector3.down, out var hitInfo, _groundCheckRayLength, groundCheckLayerMask);
            result.IsGrounded = isHitSphereCast ? Vector3.Dot(hitInfo.normal, Vector3.up) > 0.08f : false;
            Vector3 groundNormal;
            groundNormal = hitInfo.normal;
            result.GroundDistance = isHitSphereCast ? hitInfo.distance : _groundCheckRayLength;

            if (isHitSphereCast == false) // レイが地面にヒットしなかった場合、もう一度レイを飛ばす
            {
                groundCheckRayOrigin = player.transform.position + (Vector3)_groundCheckRayOffset2;
                var normal = HemisphereRaycastUtility.GetClosestHitNormalInHemisphere(groundCheckRayOrigin, Vector3.down, _groundCheckRayRadius, 1200, groundCheckLayerMask);
                result.IsGrounded = normal.HasValue;// ? Vector3.Dot(normal.Value, Vector3.up) > 0.3f : false;
                groundNormal = normal.GetValueOrDefault(Vector3.zero);
            }

            // 足元に小さなレイを飛ばして、ヒットしなかった場合には、崖にいると判定する。
            var abyssCheckRayOrigin = player.transform.position + (Vector3)_abyssCheckRayOffset;
            result.IsAbyss = !UnityEngine.Physics.SphereCast(abyssCheckRayOrigin, _abyssCheckRayRadius, Vector3.down, out var abyssHit, _abyssCheckRayLength, groundCheckLayerMask);

            if (result.IsGrounded)
            {
                result.GroundNormal = groundNormal;
                var slopeLimit = player.CharacterController.slopeLimit;
                result.IsSteepSlope = Vector3.Angle(Vector3.up, groundNormal) > slopeLimit;
                if (!result.IsSteepSlope)
                {
                    _steepSlopeCount = 0;
                }
                else if (result.IsSteepSlope && _steepSlopeCount > 5)
                {
                    result.IsSteepSlope = true;
                }
                else
                {
                    _steepSlopeCount++;
                    result.IsSteepSlope = false;
                }
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

            return result;
        }

        public void DrawGizmos(PlayerController player)
        {
            var result = CalculateGroundState(player);

            if (_isGroundCheckGizmoEnabled)
            {
                var groundCheckRayPosition = player.transform.position + (Vector3)_groundCheckRayOffset1;
                var rayLength = result.IsGrounded ? result.GroundDistance : _groundCheckRayLength;
                var groundCheckRayEnd = groundCheckRayPosition + Vector3.down * rayLength;

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

                Gizmos.color = IsAbove(player) ? new Color(1, 0, 0, _isAboveCheckGizmoAlpha) : new Color(0, 1, 0, _isAboveCheckGizmoAlpha);

                Gizmos.DrawLine(aboveCheckRayPosition, aboveCheckRayEnd);
            }

            if (_isGrabbablePointCheckGizmoEnabled)
            {
                var position = player.transform.position + player.transform.rotation * (Vector3)_grabbablePointOffset;
                var grabPointEnd = position + player.transform.rotation * Vector3.forward * _grabbablePointLength;
                Gizmos.color = GetGrabbablePoint(player) ? new Color(1, 0, 0, _isGrabPointCheckGizmoAlpha) : new Color(0, 1, 0, _isGrabPointCheckGizmoAlpha);
                Gizmos.DrawWireSphere(position, _grabbablePointRadius);
                Gizmos.DrawWireSphere(grabPointEnd, _grabbablePointRadius);
                Vector3 upOffset = Vector3.up * _grabbablePointRadius;
                Vector3 downOffset = Vector3.down * _grabbablePointRadius;
                Gizmos.DrawLine(position + upOffset, grabPointEnd + upOffset);
                Gizmos.DrawLine(position + downOffset, grabPointEnd + downOffset);
            }

            if (_isFrontCheckGizmoEnabled)
            {
                var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                var isFront = FrontCheck(player, sign);
                Gizmos.color = isFront ? new Color(1, 0, 0, _isFrontCheckGizmoAlpha) : new Color(0, 1, 0, _isFrontCheckGizmoAlpha);

                var dir = player.DirectionController.CurrentDirection == Direction.Right ? Vector3.right : Vector3.left;
                var length = _frontCheckRayLength;
                var center = player.transform.position + (Vector3)_frontCheckRayOffset + dir * length;

                Gizmos.DrawCube(center, _frontCheckBoxRayHalfSize * 2f);
            }
        }
    }

    public struct GroundSensorResult
    {
        public bool IsGrounded; // 地面と接地しているか
        public bool IsAbyss; // 崖にいるか
        public bool IsSteepSlope; // 急斜面にいるか

        public Vector2 GroundNormal; // 地面の法線
        public float GroundDistance; // 地面までの距離

        public GroundType GroundType;
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