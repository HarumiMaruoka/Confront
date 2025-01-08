using Confront.Input;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Debugger
{
    public class DebugGUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController = default;
        [SerializeField]
        private int _guiFontSize = 32;

        private void OnGUI()
        {
            var guiStyle = new GUIStyle();
            guiStyle.fontSize = _guiFontSize;

            var sensorResult = _playerController.Sensor.CalculateGroundState(_playerController);
            GUILayout.Label($"CurrentState:{_playerController.StateMachine.CurrentState.GetType().Name}", guiStyle);
            GUILayout.Label($"Velocity:{_playerController.MovementParameters.Velocity}", guiStyle);
            GUILayout.Label($"IsGrounded:{sensorResult.IsGrounded}", guiStyle);
            GUILayout.Label($"IsAbyss:{sensorResult.IsAbyss}", guiStyle);
            GUILayout.Label($"IsSteepSlope:{sensorResult.IsSteepSlope}", guiStyle);
            GUILayout.Label($"GroundNormal:{sensorResult.GroundNormal}", guiStyle);
            GUILayout.Label($"GroundNormalAngle: {Vector3.Angle(Vector3.up, sensorResult.GroundNormal)}", guiStyle);
            var leftStickInput = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
            var leftStick = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
            GUILayout.Label($"LeftStick:{(leftStick).ToString("0.00")}", guiStyle);

            guiStyle = new GUIStyle(GUI.skin.toggle);
            guiStyle.fontSize = _guiFontSize;
            guiStyle.normal.textColor = Color.black;
            DebugParams.Instance.CanPlayerAttack = GUILayout.Toggle(DebugParams.Instance.CanPlayerAttack, "CanPlayerAttack", guiStyle);
            DebugParams.Instance.StateTransitionLogging = GUILayout.Toggle(DebugParams.Instance.StateTransitionLogging, "StateTransitionLogging", guiStyle);

            guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fontSize = 40;

            // スピード変更
            if (GUILayout.Button("Speed 0.1", guiStyle)) Time.timeScale = 0.1f;
            if (GUILayout.Button("Speed 0.5", guiStyle)) Time.timeScale = 0.5f;
            if (GUILayout.Button("Speed 1", guiStyle)) Time.timeScale = 1;
            if (GUILayout.Button("Speed 2", guiStyle)) Time.timeScale = 2;

            // フレームレート変更
            if (GUILayout.Button("FrameRate 15", guiStyle)) Application.targetFrameRate = 15;
            if (GUILayout.Button("FrameRate 30", guiStyle)) Application.targetFrameRate = 30;
            if (GUILayout.Button("FrameRate 60", guiStyle)) Application.targetFrameRate = 60;
            if (GUILayout.Button("FrameRate 120", guiStyle)) Application.targetFrameRate = 120;
            if (GUILayout.Button("FrameRate 999", guiStyle)) Application.targetFrameRate = 999;
        }
    }
}