using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public class ProjectileBase : MonoBehaviour
    {
        [NonSerialized] public Vector3 TargetPosition;
        [NonSerialized] public float ActorAttackPower;

        public virtual void Launch() { }
    }
}