using Cinemachine;
using Confront.Audio;
using Confront.CameraUtilites;
using Confront.GameUI;
using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Stage
{
    public class StageController : MonoBehaviour
    {
        public AudioClip BGMClip;
        public Transform[] StartPoints;
        [HideInInspector]
        public StageTransitionData[] StageTransitionData;
        public PolygonCollider2D CameraArea;
        public float CameraDistance = 12f;

        private void OnEnable()
        {
            CameraUtilites.ConfinerHandler.SetPolygonCollider(CameraArea);
            if (!StageManager.CurrentStage) StageManager.CurrentStage = this;
            AudioManager.PlayBGM(BGMClip, 1f);
            CinemachineTransposerHandler.Instance.SetCameraDistance(CameraDistance);
        }

        private void Update()
        {
            foreach (var data in StageTransitionData)
            {
                if (data.IsPlayerWithinBounds())
                {
                    StageManager.ChangeStage(
                        data.NextStageName,
                        data.StartPointIndex,
                        ScreenFader.Instance.FadeOut,
                        ScreenFader.Instance.FadeIn).Forget();
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var data in StageTransitionData)
            {
                data.DrawGizmos();
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

        public bool IsPlayerWithinBounds()
        {
            if (!PlayerController) return false;
            var position = PlayerController.transform.position;

            return position.x >= TransitionPoint.x - HitBoxHalfSize.x && position.x <= TransitionPoint.x + HitBoxHalfSize.x
                && position.y >= TransitionPoint.y - HitBoxHalfSize.y && position.y <= TransitionPoint.y + HitBoxHalfSize.y;
        }

        public void DrawGizmos()
        {
            Gizmos.color = IsPlayerWithinBounds() ? Color.red : Color.green;
            Gizmos.DrawWireCube(TransitionPoint, HitBoxHalfSize * 2);
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
            // プロパティ変更検出開始
            UnityEditor.EditorGUI.BeginChangeCheck();

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

            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                UnityEditor.SceneView.RepaintAll();
                UnityEditor.EditorUtility.SetDirty(_stage);
            }
        }
    }
#endif
}