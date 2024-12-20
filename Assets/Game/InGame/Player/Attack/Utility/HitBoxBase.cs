using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public abstract class HitBoxBase
    {
        [Header("HitBox")]
        [SerializeField]
        protected Vector3 _offset;
        [SerializeField]
        protected Vector3 _size;
        [SerializeField]
        protected GizmoOption _gizmoOption;

        public abstract bool IsOverlapping(Transform center, LayerMask layerMask);

        public static Vector2 CalcDamageVector(Vector2 direction, float force, float sign)
        {
            sign = Mathf.Sign(sign);
            direction.x *= sign;
            if (direction.y > 0f) direction.y *= 4;
            return direction.normalized * force;
        }

        public abstract void Clear();

        public abstract void DrawGizmos(Transform center, float elapsed, LayerMask layerMask);
    }
}