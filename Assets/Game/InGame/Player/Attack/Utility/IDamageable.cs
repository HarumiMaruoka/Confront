using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public interface IDamageable
    {
        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="attackPower"> ダメージ量 </param>
        /// <param name="damageVector"> ノックバックに使用する </param>
        void TakeDamage(float attackPower, Vector2 damageVector);
    }
}