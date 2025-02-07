using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    [CreateAssetMenu(fileName = "Eye", menuName = "ConfrontSO/Enemy/Eye")]
    public class EnemyEye : ScriptableObject
    {
        [Header("視界設定")]
        public float ViewDistance;
        public float ViewAngle;

        [Header("座標オフセット")]
        public Vector2 Offset;
        [Header("回転オフセット")]
        public float AngleOffset;

        private bool _isVisibleRecord;
        private RaycastHit _hit;

        public bool IsGizmosVisible = true;

        public bool IsVisiblePlayer(Transform eyeTransform, PlayerController player)
        {
            // エリア外にいるプレイヤーは見えない
            var origin = eyeTransform.position + (Vector3)Offset;
            var playerCenter = player.transform.position + player.CharacterController.center;

            var sqrDistance = (origin - playerCenter).sqrMagnitude;
            if (sqrDistance > ViewDistance * ViewDistance) return _isVisibleRecord = false;

            var viewDirection = Quaternion.Euler(0, 0, AngleOffset) * eyeTransform.forward;

            // プレイヤーが視界角内にいるかどうか
            var direction = playerCenter - origin;
            var angle = Vector3.Angle(viewDirection, direction);
            if (angle > ViewAngle / 2f) return _isVisibleRecord = false;

            // プレイヤーが障害物に隠れていないかどうか
            if (Physics.Raycast(origin, direction, out _hit, ViewDistance, LayerUtility.PlayerLayerMask))
            {
                if (_hit.collider.transform == player.transform) return _isVisibleRecord = true;
            }
            return _isVisibleRecord = false;
        }

        public void DrawGizmos(Transform transform)
        {
            // 視界の描画
            if (!IsGizmosVisible) return;

            Gizmos.color = _isVisibleRecord ? Color.red : Color.blue;

            var viewDirection = Quaternion.Euler(0, 0, AngleOffset) * transform.forward;

            var halfAngle = ViewAngle / 2;

            var top = transform.position + (Vector3)Offset + ((Quaternion.Euler(0, 0, halfAngle) * viewDirection) * ViewDistance);
            var bottom = transform.position + (Vector3)Offset + ((Quaternion.Euler(0, 0, -halfAngle) * viewDirection) * ViewDistance);

            Gizmos.DrawLine(transform.position + (Vector3)Offset, top);
            Gizmos.DrawLine(transform.position + (Vector3)Offset, bottom);

            if (_hit.collider)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + (Vector3)Offset, _hit.point);
            }


#if UNITY_EDITOR
            UnityEditor.Handles.color = _isVisibleRecord ? Color.red : Color.blue;

            // 弧を描くためのパラメータ設定
            var center = transform.position + (Vector3)Offset;
            var up = transform.right; // 弧の法線軸として使用

            // DrawWireArc(中心, 法線, 開始方向ベクトル, 角度, 半径)
            // 開始方向を視野角の左端とするために、-ViewAngle/2だけ回転させる
            Vector3 startDirection = Quaternion.AngleAxis(-ViewAngle / 2f, up) * viewDirection;
            UnityEditor.Handles.DrawWireArc(center, up, startDirection, ViewAngle, ViewDistance);

            // 視線のヒット結果をラインで描画
            if (_hit.collider != null)
            {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawLine(center, _hit.point);
            }
#endif
        }
    }
}