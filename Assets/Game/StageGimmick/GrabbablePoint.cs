using Confront.Player;
using System;
using UnityEngine;

namespace Confront.StageGimmick
{
    public class GrabbablePoint : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 360f)]
        private float _groundAngle;

        public float GroundAngle => _groundAngle;
        public Vector2 ClimbedPosition
        {
            get
            {
                // var distance = MovementParameters.Instance.ClimbedPositionDistance;
                //return (Vector2)transform.position + (Vector2)(Quaternion.Euler(0f, 0f, _groundAngle) * Vector2.up * distance);
                return default;
            }
        }

#if UNITY_EDITOR
        public const string gizmoName = "Grab Point";

        private void OnDrawGizmos()
        {
            //if (!UnityEditor.EditorPrefs.GetBool(gizmoName)) return;

            //Vector3 direction = Quaternion.Euler(0f, 0f, _groundAngle) * Vector3.up * MovementParameters.Instance.ClimbedPositionDistance;
            //Vector3 arrowHead = transform.position + direction;

            //// Handlesの色を設定
            //UnityEditor.Handles.color = Color.blue;

            //// 矢印の線を描画
            //UnityEditor.Handles.DrawLine(transform.position, arrowHead);

            //// 矢印の先端を描画
            //float arrowHeadSize = 0.15f;
            //Vector3 right = Quaternion.Euler(0f, 0f, 135f) * direction.normalized * arrowHeadSize;
            //Vector3 left = Quaternion.Euler(0f, 0f, -135f) * direction.normalized * arrowHeadSize;
            //UnityEditor.Handles.DrawLine(arrowHead, arrowHead + right);
            //UnityEditor.Handles.DrawLine(arrowHead, arrowHead + left);
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