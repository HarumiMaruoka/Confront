using Confront.StageGimmick;
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
        public Vector2 _groundCheckRayOffset = new Vector2(0, 0f);
        [SerializeField]
        private float _angleRange = 120f;
        [SerializeField]
        private int _rayCount = 52;
        [SerializeField]
        public float _rayDistance = 0.58f;
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
            LayerMask groundCheckLayerMask = GroundLayerMask;

            if (player.StateMachine?.CurrentState is not GroundDodge)
                groundCheckLayerMask |= EnemyLayerMask;

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

        public Vector2? GetGroundPoint(PlayerController player)
        {
            var groundCheckRayOrigin = player.transform.position + (Vector3)_groundCheckRayOffset + new Vector3(0, 0.5f);
            var groundCheckLayerMask = GroundLayerMask | EnemyLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished) groundCheckLayerMask |= PassThroughPlatform;

            return UnityEngine.Physics.Raycast(groundCheckRayOrigin, Vector3.down, out var hit, _rayDistance + 1f, groundCheckLayerMask) ? hit.point : null;
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

        public GroundSensorResult CalculateGroundState(PlayerController player)
        {
            var result = new GroundSensorResult();

            var groundCheckRayOrigin = player.transform.position + (Vector3)_groundCheckRayOffset;
            var abyssCheckRayOrigin = player.transform.position + (Vector3)_abyssCheckRayOffset;
            var slopeLimit = player.CharacterController.slopeLimit;

            LayerMask groundCheckLayerMask;

            if (player.MovementParameters.IsPassThroughPlatformTimerFinished)
                groundCheckLayerMask = GroundLayerMask | EnemyLayerMask | PassThroughPlatform;
            else
                groundCheckLayerMask = GroundLayerMask | EnemyLayerMask;


            // 足元にレイを飛ばして、ヒットした場合には、地面にいると判定する。
            var groundNormal = CastFanRaysAndGetAveragedNormal(player, Vector3.down, groundCheckLayerMask);
            var groundAngle = Vector3.Angle(Vector3.up, groundNormal);
            result.IsGrounded = groundNormal != Vector3.zero && groundAngle <= 90f;

            // 足元に小さなレイを飛ばして、ヒットしなかった場合には、崖にいると判定する。
            result.IsAbyss = !UnityEngine.Physics.SphereCast(abyssCheckRayOrigin, _abyssCheckRayRadius, Vector3.down, out var abyssHit, _abyssCheckRayLength, groundCheckLayerMask);

            if (result.IsGrounded)
            {
                result.GroundNormal = groundNormal;
                result.IsSteepSlope = Vector3.Angle(Vector3.up, groundNormal) > slopeLimit;
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

        private List<Vector3> _hitNormals = new List<Vector3>();

        private Vector3 CastFanRaysAndGetAveragedNormal(PlayerController player, Vector3 direction, LayerMask groundCheckLayerMask)
        {
            var origin = player.transform.position + (Vector3)_groundCheckRayOffset;

            _hitNormals.Clear();
            rayInfos.Clear(); // 前回のRay情報をクリア
            direction = direction.normalized;

            float halfAngle = _angleRange * 0.5f;

            for (int i = 0; i < _rayCount; i++)
            {
                float t = (float)i / (_rayCount - 1);
                float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

                Quaternion rot = Quaternion.Euler(0, 0, angle);
                Vector3 dir = rot * direction;

                Ray ray = new Ray(origin, dir);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit, _rayDistance, groundCheckLayerMask, QueryTriggerInteraction.Ignore);

                if (isHit)
                {
                    hit.normal = new Vector3(hit.normal.x, hit.normal.y, 0);
                    _hitNormals.Add(hit.normal);
                    rayInfos.Add(new RayInfo { start = origin, end = hit.point, hit = true });
                }
                else
                {
                    rayInfos.Add(new RayInfo { start = origin, end = origin + dir * _rayDistance, hit = false });
                }
            }

            if (_hitNormals.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 sum = Vector3.zero;
            foreach (var normal in _hitNormals)
            {
                sum += normal;
            }
            Vector3 averageNormal = sum / _hitNormals.Count;
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

    public struct GroundSensorResult
    {
        public bool IsGrounded; // 地面と接地しているか
        public bool IsAbyss; // 崖にいるか
        public bool IsSteepSlope; // 急斜面にいるか

        public Vector2 GroundNormal; // 地面の法線

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