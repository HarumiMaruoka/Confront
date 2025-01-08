using System;
using UnityEngine;

namespace Confront.Player
{
    [UnityEditor.CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        private PlayerController _playerController;

        private string MovementParametersKey = "isMovementParametersFoldout";
        private string SensorKey = "isSensorFoldout";
        private string CharacterStatsKey = "isCharacterStatsFoldout";

        private void OnEnable()
        {
            _playerController = (PlayerController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawFoldout("Character Stats", _playerController.CharacterStats, CharacterStatsKey);
            DrawFoldout("Movement Parameters", _playerController.MovementParameters, MovementParametersKey);
            DrawFoldout("Sensor", _playerController.Sensor, SensorKey);
        }

        private void DrawFoldout(string label, UnityEngine.Object targetObject, string prefsKey)
        {
            bool isFoldout = UnityEditor.EditorPrefs.GetBool(prefsKey, false);
            isFoldout = UnityEditor.EditorGUILayout.Foldout(isFoldout, label);
            UnityEditor.EditorPrefs.SetBool(prefsKey, isFoldout);

            if (isFoldout && targetObject)
            {
                UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(targetObject);
                serializedObject.Update();
                UnityEditor.SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);

                while (iterator.NextVisible(false))
                {
                    UnityEditor.EditorGUILayout.PropertyField(iterator, true);
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}