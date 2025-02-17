using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public interface IDamageable
    {
        GameObject Owner { get; }
        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="attackPower"> ダメージ量 </param>
        /// <param name="damageVector"> ノックバックに使用する </param>
        /// <param name="point"></param>
        void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point);
    }
}