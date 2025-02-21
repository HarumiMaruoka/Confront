using System;
using UnityEngine;
using Confront.Player;
using Confront.AttackUtility;
using Confront.Utility;

namespace Confront.Enemy.Slimey
{
    // ターゲットに向かって勢いよく突進し、ぶつかることでダメージを与える攻撃行動です。スライムの攻撃の中で最も特徴的な行動です。
    [CreateAssetMenu(fileName = "AttackState", menuName = "ConfrontSO/Enemy/Slimey/States/AttackState")]
    public class AttackState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "Attack02";

        [SerializeField]
        private HitBox[] _hitBoxes;

        [SerializeField]
        private Projectile[] _projectiles;

        public float Duration = 3.0f;
        public float AttackRange = 1.0f;

        private float _elapsedTime;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            foreach (var hitBox in _hitBoxes) hitBox.Clear();
            _elapsedTime = 0;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            var prevElapsed = _elapsedTime;
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > Duration)
            {
                slimey.ChangeState<ApproachState>();
            }

            foreach (var hitBox in _hitBoxes)
            {
                var direction = slimey.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                hitBox.Update(slimey.transform, slimey.Stats.AttackPower, direction, _elapsedTime, LayerUtility.PlayerLayerMask, false);
            }

            foreach (var projectile in _projectiles)
            {
                if (_elapsedTime > projectile.FireTime && prevElapsed <= projectile.FireTime)
                {
                    var spawnPoint = slimey.GetSpawnPoint(projectile.SpawnPointIndex);

                    var instance = Instantiate(projectile.Prefab, spawnPoint, Quaternion.identity);
                    instance.TargetPosition = player.transform.position;
                    instance.ActorAttackPower = slimey.Stats.AttackPower;
                    instance.Launch();
                }
            }
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {
            _elapsedTime = -1f;
        }

        public override void DrawGizmos(PlayerController player, SlimeyController slimey)
        {
            if (_hitBoxes == null) return;
            foreach (var hitBox in _hitBoxes)
            {
                hitBox.DrawGizmos(slimey.transform, _elapsedTime, LayerUtility.PlayerLayerMask);
            }
        }
    }

    [Serializable]
    public struct Projectile
    {
        public ProjectileBase Prefab;
        public int SpawnPointIndex;
        public float FireTime;
    }
}