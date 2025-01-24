using System;
using UnityEngine;

namespace Confront.Title
{
    public class TitleCameraController : MonoBehaviour
    {
        public Transform target; // 注視対象
        public float orbitSpeed = 10f; // 楕円軌道の速度
        public Vector3 orbitSize = new Vector3(5f, 2f, 5f); // 楕円の大きさ
        public float waveAmplitude = 1f; // 上下の波の振幅
        public float waveFrequency = 1f; // 波の周波数
        public float zoomSpeed = 1f; // ズームの速度
        public float zoomRange = 2f; // ズームの振幅

        void Start()
        {
            if (target == null)
            {
                Debug.LogError("ターゲットが設定されていません。");
                enabled = false;
                return;
            }
        }

        void LateUpdate()
        {
            // 時間の進行に基づく計算
            float time = Time.time;

            // 楕円軌道の計算
            float x = Mathf.Cos(time * orbitSpeed) * orbitSize.x;
            float z = Mathf.Sin(time * orbitSpeed) * orbitSize.z;

            // 上下の波の動き
            float y = Mathf.Sin(time * waveFrequency) * waveAmplitude;

            // ズームの計算
            float zoom = Mathf.Sin(time * zoomSpeed) * zoomRange;

            // 新しいカメラ位置を計算
            Vector3 offset = new Vector3(x, y, z);
            transform.position = target.position + offset.normalized * (orbitSize.magnitude + zoom);

            // ターゲットを注視
            transform.LookAt(target.position);
        }
    }
}