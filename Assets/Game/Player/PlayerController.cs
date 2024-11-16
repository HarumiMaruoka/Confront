using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;

        public MovementParameters MovementParameters;
        public Sensor Sensor;
        public StateMachine StateMachine;
        public DirectionController DirectionController;
        public Animator Animator;

        public CharacterController CharacterController { get => _characterController ??= GetComponent<CharacterController>(); }

        private void Start()
        {
            DirectionController.Initialize(transform);
            StateMachine = new StateMachine(this);
            StateMachine.ChangeState<Grounded>();
        }

        private void Update()
        {
            StateMachine.Update();
            if (CharacterController.enabled) CharacterController.Move(MovementParameters.Velocity * Time.deltaTime);
            Animator.SetFloat("RunSpeed", Mathf.Abs(MovementParameters.Velocity.x / MovementParameters.MaxSpeed));
            DirectionController.UpdateVelocity(MovementParameters.Velocity);

            if (PlayerInputHandler.InGameInput.Jump.triggered)
            {
                StateMachine.ChangeState<Jump>();
            }
            if (MovementParameters.GrabIntervalTimer > 0)
            {
                MovementParameters.GrabIntervalTimer -= Time.deltaTime;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Sensor) Sensor.DrawGizmos(this);
        }
#endif

        [SerializeField]
        private int _guiFontSize = 32;

        private void OnGUI()
        {
            var guiStyle = new GUIStyle();
            guiStyle.fontSize = _guiFontSize;

            var sensorResult = Sensor.Calculate(this);
            GUILayout.Label($"CurrentState:{StateMachine.CurrentState.GetType().Name}", guiStyle);
            GUILayout.Label($"Velocity:{MovementParameters.Velocity}", guiStyle);
            GUILayout.Label($"IsGrounded:{sensorResult.IsGrounded}", guiStyle);
            GUILayout.Label($"IsAbyss:{sensorResult.IsAbyss}", guiStyle);
            GUILayout.Label($"IsSteepSlope:{sensorResult.IsSteepSlope}", guiStyle);
            GUILayout.Label($"IsAbove:{sensorResult.IsAbove}", guiStyle);
            var leftStickInput = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
            var leftStick = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
            GUILayout.Label($"LeftStick:{(leftStick).ToString("0.00")}", guiStyle);

            guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.fontSize = 40;

            if (GUILayout.Button("Speed 0.1", guiStyle)) Time.timeScale = 0.1f;
            if (GUILayout.Button("Speed 0.5", guiStyle)) Time.timeScale = 0.5f;
            if (GUILayout.Button("Speed 1", guiStyle)) Time.timeScale = 1;
            if (GUILayout.Button("Speed 2", guiStyle)) Time.timeScale = 2;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        private PlayerController _playerController;

        private string MovementParametersKey = "isMovementParametersFoldout";
        private string SensorKey = "isSensorFoldout";

        private void OnEnable()
        {
            _playerController = (PlayerController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

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
#endif
}