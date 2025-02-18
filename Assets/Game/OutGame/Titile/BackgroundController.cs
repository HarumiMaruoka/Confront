using System;
using UnityEngine;

namespace Confront.Title
{
    public class BackgroundController : MonoBehaviour
    {
        // 移動範囲（左右上下±moveRange）
        public float moveRange = 10f;
        // ランダムな速度の範囲（最小、最大）
        public Vector2 speedRange = new Vector2(0.5f, 2f);
        // 移動の滑らかさ
        public float smoothTime = 1f;

        private RectTransform rectTransform;  // UIオブジェクトのRectTransform
        private Vector2 initialPosition;      // 初期位置を記録
        private Vector2 targetPosition;       // 目標位置
        private Vector2 velocity = Vector2.zero; // SmoothDamp用の速度変数
        private float currentSpeed;           // 今回の移動速度

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform が見つかりません。UIオブジェクトにアタッチしてください。");
                return;
            }

            // 初期位置を記録
            initialPosition = rectTransform.anchoredPosition;
            ChooseNewTarget();
        }

        void Update()
        {
            // RectTransformの anchoredPosition をスムーズに更新
            rectTransform.anchoredPosition = Vector2.SmoothDamp(
                rectTransform.anchoredPosition, targetPosition, ref velocity, smoothTime, currentSpeed
            );

            // 目標位置にほぼ到達したら新たな目標を設定
            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                ChooseNewTarget();
            }
        }

        // 新しい目標位置と速度を設定
        void ChooseNewTarget()
        {
            float randomX = UnityEngine.Random.Range(initialPosition.x - moveRange, initialPosition.x + moveRange);
            float randomY = UnityEngine.Random.Range(initialPosition.y - moveRange, initialPosition.y + moveRange);
            targetPosition = new Vector2(randomX, randomY);

            currentSpeed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
        }
    }
}