using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public interface IDamageable
    {
        void TakeDamage(float attackPower);
    }
}