using Confront.Debugger;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    // ランダムに飛行する。
    [CreateAssetMenu(fileName = "Fly", menuName = "ConfrontSO/Enemy/VampireBat/States/Fly")]
    public class Fly : VampireBatState
    {
        public float OrbitRadius = 60f;
        public float MoveSpeed = 0.5f;
        public float NoiseScale = 1f;
        public float Offset = 1.5f;

        private float _noiseOffsetX;
        private float _noiseOffsetY;

        public float MinFlightTime = 1.0f;
        public float AnimationTransitionDuration = 0.8f;

        private float _flightTime = 0f;
        private bool IsFlightTimeOver => _flightTime > MinFlightTime;

        public override string AnimationName => "FlyStart";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {
            _flightTime = 0f;
            // ノイズのオフセットをランダムに設定
            _noiseOffsetX = UnityEngine.Random.Range(0f, 100f);
            _noiseOffsetY = UnityEngine.Random.Range(0f, 100f);
        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            if (vampireBat.Eye.IsVisiblePlayer(vampireBat.transform, player))
            {
                vampireBat.ChangeState<Approach>();
                return;
            }

            _flightTime += Time.deltaTime;
            if (_flightTime < AnimationTransitionDuration)
            {
                vampireBat.Animator.CrossFade("FlyCycle", 0.1f);
            }
            if (IsFlightTimeOver && vampireBat.Sensor.IsGrounded(vampireBat.transform, out RaycastHit hit))
            {
                vampireBat.ChangeState<Idle>();
                return;
            }

            Move(vampireBat);
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }

        private void Move(VampireBatController vampireBat)
        {
            // Perlinノイズを使用してランダムなオフセットを生成
            float noiseX = Mathf.PerlinNoise(Time.time * NoiseScale + _noiseOffsetX, 0f) - 0.5f;
            float noiseY = Mathf.PerlinNoise(0f, Time.time * NoiseScale + _noiseOffsetY) - 0.5f;

            // 円状の範囲内での新しい位置を計算
            float angle = Mathf.Atan2(noiseY, noiseX);
            float radius = OrbitRadius * Mathf.Sqrt(noiseX * noiseX + noiseY * noiseY);
            float x = vampireBat.OrbitCenter.transform.position.x + Mathf.Cos(angle) * radius;
            float y = vampireBat.OrbitCenter.transform.position.y + Mathf.Sin(angle) * radius;

            // ファンネルの位置を更新
            // vampireBat.transform.position = Vector2.Lerp(vampireBat.transform.position, new Vector2(x, y), MoveSpeed * Time.deltaTime);
            vampireBat.Rigidbody.MovePosition(Vector2.Lerp(vampireBat.transform.position, new Vector2(x, y), MoveSpeed * Time.deltaTime));
        }
    }
}