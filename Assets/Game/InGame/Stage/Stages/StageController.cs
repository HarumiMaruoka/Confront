using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Stage
{
    public class StageController : MonoBehaviour
    {
        public Transform[] StartPoints;
        [HideInInspector]
        public StageTransitionData[] StageTransitionData;

        private string[] _connectedStages;
        public string[] ConnectedStages => _connectedStages ??= Array.ConvertAll(StageTransitionData, x => x.NextStageName);

        private void OnValidate()
        {
            _connectedStages = null;
        }

        private void OnDrawGizmos()
        {
            foreach (var data in StageTransitionData)
            {
                data.DrawGizmos(transform);
            }
        }
    }

    [Serializable]
    public class StageTransitionData
    {
        public string NextStageName => NextStageType.ToString() + StageNumber.ToString();
        public StageType NextStageType;
        public int StageNumber;
        public int StartPointIndex;
        public Vector3 TransitionPoint;
        public Vector3 HitBoxHalfSize;

        private PlayerController PlayerController => PlayerController.Instance;

        public bool IsHit()
        {
            var position = PlayerController.transform.position;

            return position.x >= TransitionPoint.x - HitBoxHalfSize.x && position.x <= TransitionPoint.x + HitBoxHalfSize.x
                && position.y >= TransitionPoint.y - HitBoxHalfSize.y && position.y <= TransitionPoint.y + HitBoxHalfSize.y;
        }

        public void DrawGizmos(Transform center)
        {
            Gizmos.color = IsHit() ? Color.red : Color.green;
            Gizmos.DrawWireCube(center.position + TransitionPoint, HitBoxHalfSize * 2);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(StageController))]
    public class StageEditor : UnityEditor.Editor
    {
        private StageController _stage;

        private void OnEnable()
        {
            _stage = target as StageController;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_stage.StageTransitionData == null)
            {
                _stage.StageTransitionData = new StageTransitionData[0];
            }

            for (int i = 0; i < _stage.StageTransitionData.Length; i++)
            {
                var data = _stage.StageTransitionData[i];

                UnityEditor.EditorGUILayout.Space();

                UnityEditor.EditorGUILayout.BeginHorizontal();
                UnityEditor.EditorGUILayout.LabelField($"Transition {i + 1}");
                if (GUILayout.Button("Remove"))
                {
                    UnityEditor.ArrayUtility.RemoveAt(ref _stage.StageTransitionData, i);
                    UnityEditor.EditorUtility.SetDirty(_stage);
                    return;
                }
                UnityEditor.EditorGUILayout.EndHorizontal();

                data.NextStageType = (StageType)UnityEditor.EditorGUILayout.EnumPopup("Next Stage Type", data.NextStageType);
                data.StageNumber = UnityEditor.EditorGUILayout.IntField("Stage Number", data.StageNumber);
                data.StartPointIndex = UnityEditor.EditorGUILayout.IntField("Start Point Index", data.StartPointIndex);
                data.TransitionPoint = UnityEditor.EditorGUILayout.Vector3Field("Transition Point", data.TransitionPoint);
                data.HitBoxHalfSize = UnityEditor.EditorGUILayout.Vector3Field("Hit Box Half Size", data.HitBoxHalfSize);

                _stage.StageTransitionData[i] = data;
            }

            if (GUILayout.Button("Add Transition"))
            {
                UnityEditor.ArrayUtility.Add(ref _stage.StageTransitionData, new StageTransitionData());
                UnityEditor.EditorUtility.SetDirty(_stage);
            }
        }
    }
#endif
}