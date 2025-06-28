using Confront.Enemy;
using Confront.GUI;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public class DestructiblePart : BossPart
    {
        public float Health; // このパーツの体力
        public float DestructedDefense; // 破壊された状態のとき防御力

        private float _destructionLevel = 0f; // 破壊度を表す。[0: 未破壊, 1: 完全破壊]
        private float _totalDamage = 0f; // このパーツがこれまでに受けたダメージの合計

        public event Action<DestructiblePart> OnDestructed; // 破壊されたときに呼ばれるイベント

        public override void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point)
        {
            // ダメージを計算
            float damage;
            if (_destructionLevel < 1) damage = EnemyBase.DefaultCalculateDamage(attackPower, Defense);
            else damage = EnemyBase.DefaultCalculateDamage(attackPower, DestructedDefense);

            // ボスへダメージを与える
            Boss.TakeDamage(damage, damageVector, point);

            // 破壊度を更新
            var prevDestructionLevel = _destructionLevel;
            _totalDamage += damage;
            _destructionLevel = (_totalDamage / Health);

            // 破壊されたときのイベントを実行
            if (prevDestructionLevel < 1 && _destructionLevel >= 1) OnDestructed?.Invoke(this);
        }
    }
}