using System;
using UnityEngine;

namespace Confront.Physics
{
    [CreateAssetMenu(fileName = "GroundSensor", menuName = "Confront/Physics/GroundSensor")]
    public class GroundSensor : ScriptableObject
    {
        [SerializeField]
        private LayerMask _groundLayer;

        [SerializeField]
        public float _groundCheckSphereRadius = 0.15f;
        [SerializeField]
        private Vector2 _groundCheckSphereOffset = new Vector2(0f, 1f);
        [SerializeField]
        private float _groundCheckDistance = 0.87f;

        [SerializeField]
        private Vector2 _abyssCheckSphereOffset = new Vector2(0f, 1f);
        [SerializeField]
        private float _abyssCheckSphereRadius = 0.1f;
        [SerializeField]
        private float _abyssCheckDistance = 1.2f;

        public GroundSensorResult CheckGround(Vector2 position, Vector2 direction, float slopeLimit, bool IsAscendingDueToJump)
        {
            var result = new GroundSensorResult();

            var groundCheckSpherePosition = position + _groundCheckSphereOffset;
            var abyssCheckSpherePosition = position + _abyssCheckSphereOffset;

            // 足元にレイを飛ばして、ヒットした場合には、地面にいると判定する。
            result.IsGrounded = UnityEngine.Physics.SphereCast(groundCheckSpherePosition, _groundCheckSphereRadius, direction, out var groundHit, _groundCheckDistance, _groundLayer);
            // 足元に小さなレイを飛ばして、ヒットしなかった場合には、崖にいると判定する。
            result.IsAbyss = !UnityEngine.Physics.SphereCast(abyssCheckSpherePosition, _abyssCheckSphereRadius, direction, out var abyssHit, _abyssCheckDistance, _groundLayer);

            if (result.IsGrounded)
            {
                result.GroundNormal = groundHit.normal;
                result.GroundPoint = groundHit.point;
                result.IsSteepSlope = Vector3.Angle(Vector3.up, groundHit.normal) > slopeLimit;
            }

            result.GroundState = GroundState.InAir;
            if (result.IsGrounded) result.GroundState = GroundState.Grounded;
            if (IsAscendingDueToJump) result.GroundState = GroundState.InAir;
            if (result.IsSteepSlope) result.GroundState = GroundState.SteepSlope;
            if (result.IsAbyss && result.IsGrounded) result.GroundState = GroundState.Abyss;

            return result;
        }

        public void DrawGizmos(Vector2 position, Vector2 direction, float slopeLimit)
        {
            var result = CheckGround(position, direction, slopeLimit, default);

            if (result.IsGrounded) Gizmos.color = new Color(1, 0, 0, 0.5f);
            else Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(position + _groundCheckSphereOffset + direction * _groundCheckDistance, _groundCheckSphereRadius);

            if (result.IsAbyss) Gizmos.color = new Color(0, 1, 1, 0.5f);
            else Gizmos.color = new Color(1, 0, 1, 0.5f);
            Gizmos.DrawSphere(position + _abyssCheckSphereOffset + direction * _abyssCheckDistance, _abyssCheckSphereRadius);
        }
    }

    public struct GroundSensorResult
    {
        public bool IsGrounded; // 地面
        public bool IsAbyss; // 崖
        public bool IsSteepSlope; // 急斜面
        public GroundState GroundState;
        public Vector3 GroundPoint;
        public Vector2 GroundNormal;
    }

    public enum GroundState
    {
        Grounded,
        InAir,
        Abyss,
        SteepSlope,
    }
}