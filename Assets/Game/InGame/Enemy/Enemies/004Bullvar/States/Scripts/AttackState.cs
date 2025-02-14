using Confront.AttackUtility;
using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 攻撃する。
    // ダメージを食らったら確率でDamageに遷移する。
    // アニメーションが終了したらBlockかApproachに遷移する。
    [CreateAssetMenu(fileName = "AttackState", menuName = "ConfrontSO/Enemy/Bullvar/States/AttackState")]
    public class AttackState : BullvarState
    {
        [SerializeField]
        private string _animationName = "Attack_01";

        public HitBox[] HitBoxes;
        public Confront.Enemy.Slimey.Projectile[] Projectiles;

        public float Deceleration = 8f;

        public float blockTransitionProbability = 0.5f;
        public float Duration = 3f;

        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _timer = 0;
            foreach (var hitBox in HitBoxes) hitBox.Clear();
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            var previousTime = _timer;
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                var random = UnityEngine.Random.value;
                if (random < blockTransitionProbability)
                {
                    bullvar.ChangeState<BlockState>();
                }
                else
                {
                    bullvar.ChangeState<ApproachState>();
                }
            }

            bullvar.Velocity.x = Mathf.Lerp(bullvar.Velocity.x, 0, Time.deltaTime * Deceleration);

            foreach (var hitBox in HitBoxes)
            {
                var direction = bullvar.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                hitBox.Update(bullvar.transform, bullvar.Stats.AttackPower, direction, _timer, LayerUtility.PlayerLayerMask, false);
            }

            foreach (var projectile in Projectiles)
            {
                if (projectile.FireTime < _timer && projectile.FireTime >= previousTime)
                {
                    var spawnPoint = bullvar.GetSpawnPoint(projectile.SpawnPointIndex);
                    if (spawnPoint == null) break;

                    var instance = Instantiate(projectile.Prefab, spawnPoint.Value, Quaternion.identity);
                    instance.TargetPosition = player.transform.position;
                    instance.ActorAttackPower = bullvar.Stats.AttackPower;
                    instance.Launch();
                }
            }
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }

        public override void DrawGizmos(PlayerController player, BullvarController bullvar)
        {
            foreach (var hitBox in HitBoxes)
            {
                hitBox.DrawGizmos(bullvar.transform, _timer, LayerUtility.PlayerLayerMask);
            }
        }
    }
}