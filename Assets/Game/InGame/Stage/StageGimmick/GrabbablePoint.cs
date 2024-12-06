using Confront.Player;
using System;
using UnityEngine;

namespace Confront.StageGimmick
{
    public class GrabbablePoint : MonoBehaviour
    {
        [SerializeField]
        private Direction _direction;

        public Direction Direction => _direction;

#if UNITY_EDITOR
        public const string gizmoName = "Grab Point";

        private void OnDrawGizmos()
        {
            if (!UnityEditor.EditorPrefs.GetBool(gizmoName)) return;

            var angle = _direction == Direction.Right ? 270f : 90f;
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.up * 0.8f;
            Vector3 arrowHead = transform.position + direction;

            // Handlesの色を設定
            UnityEditor.Handles.color = Color.blue;

            // 矢印の線を描画
            UnityEditor.Handles.DrawLine(transform.position, arrowHead);

            // 矢印の先端を描画
            float arrowHeadSize = 0.15f;
            Vector3 right = Quaternion.Euler(0f, 0f, 135f) * direction.normalized * arrowHeadSize;
            Vector3 left = Quaternion.Euler(0f, 0f, -135f) * direction.normalized * arrowHeadSize;
            UnityEditor.Handles.DrawLine(arrowHead, arrowHead + right);
            UnityEditor.Handles.DrawLine(arrowHead, arrowHead + left);
        }
#endif
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(GrabbablePoint))]
    public class GrabPointDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gizmoName = GrabbablePoint.gizmoName;
            var isDrawGizmo = UnityEditor.EditorPrefs.GetBool(gizmoName);
            var previousValue = isDrawGizmo;

            isDrawGizmo = UnityEditor.EditorGUILayout.Toggle("Draw Gizmo", isDrawGizmo);
            var isChanged = previousValue != isDrawGizmo;
            UnityEditor.EditorPrefs.SetBool(gizmoName, isDrawGizmo);

            if (isChanged)
            {
                UnityEditor.SceneView.RepaintAll();
            }
        }
    }
#endif
}