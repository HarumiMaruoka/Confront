using System;
using UnityEngine;

namespace Confront.Utility
{
    public class ProjectileMotion : MonoBehaviour
    {
        [Header("ターゲット設定")]
        [Tooltip("到達させたいターゲットのTransformを指定してください")]
        public Transform target;  // ターゲット

        [Header("飛行設定")]
        [Tooltip("飛行時間（秒）")]
        public float flightDuration = 2.0f;  // 飛行にかける時間（秒）
        [Tooltip("重力加速度（マイナス値。例: -9.8）")]
        public float gravity = -9.8f;        // 重力加速度（下方向なので負の値）

        // 内部で使用する変数
        private Vector2 startPosition;       // 発射時の位置
        private Vector2 initialVelocity;     // 算出した初速度
        private float elapsedTime = 0f;      // 経過時間

        public event Action<ProjectileMotion> OnComplete;

        public ProjectileMotion Launch(Vector2 startPosition, Transform target, float flightDuration)
        {
            this.elapsedTime = 0f;
            this.transform.position = startPosition;
            this.startPosition = startPosition;
            this.target = target;
            this.flightDuration = flightDuration;
            Initialize();
            return this;
        }

        private void Initialize()
        {
            // ターゲットが指定されていなければエラー表示
            if (target == null)
            {
                Debug.LogError("ターゲットが指定されていません！");
                enabled = false;
                return;
            }

            // 発射開始位置を保存
            startPosition = transform.position;

            // 水平方向の初速度：等速運動でターゲットに到達するための値
            float vx = (target.position.x - startPosition.x) / flightDuration;

            // 垂直方向の初速度は、重力加速度を考慮して計算する
            // 公式: target.y = start.y + v0y * t + (1/2)*g*t^2　より
            float vy = (target.position.y - startPosition.y - 0.5f * gravity * flightDuration * flightDuration) / flightDuration;

            initialVelocity = new Vector2(vx, vy);
        }

        private void Update()
        {
            // 経過時間が飛行時間以内なら座標を更新
            if (elapsedTime < flightDuration)
            {
                elapsedTime += Time.deltaTime;
                // 位置の更新：初期位置 + 初速度 * 時間 + 1/2 * 加速度 * 時間^2
                Vector2 currentPos = startPosition
                    + initialVelocity * elapsedTime
                    + 0.5f * new Vector2(0, gravity) * elapsedTime * elapsedTime;
                transform.position = currentPos;
            }
            else
            {
                // 飛行時間経過後はターゲット位置に固定
                transform.position = target.position;
                // 以降のUpdate処理を止める（必要に応じて、破棄や他の処理を追加してください）
                enabled = false;
                OnComplete?.Invoke(this);
            }
        }
    }
}