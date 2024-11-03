using Confront.Input;
using Confront.Physics;
using Confront.Stage;
using System;
using UnityEngine;

namespace Confront.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private MovementSettings _defaultMovementSettings;
        [SerializeField]
        private GroundSensor _groundSensor;
        [SerializeField]
        private float _jumpForce = 10f;

        private MovementSystem _movementSystem;
        private CharacterController _characterController;
        private PlayerStateMachine _stateMachine;
        private LayerController _layerController;

        public Animator Animator => _animator;
        public MovementSystem MovementSystem => _movementSystem;
        public MovementSettings DefaultMovementSettings => _defaultMovementSettings;
        public GroundSensor GroundSensor => _groundSensor;
        public PlayerStateMachine StateMachine => _stateMachine;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _layerController = GetComponent<LayerController>();
            _stateMachine = new PlayerStateMachine(this);
            _stateMachine.ChangeState<States.Idle>();
            _movementSystem = new MovementSystem(_characterController, _defaultMovementSettings, _groundSensor, _layerController);
        }

        private void Update()
        {
            Vector2 moveInput = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
            bool isJump = PlayerInputHandler.InGameInput.Jump.triggered;
            bool isDush = PlayerInputHandler.InGameInput.Dash.ReadValue<float>() > 0.5f;

            _movementSystem.Update(moveInput.x);
            if (isJump) _movementSystem.Jump(_jumpForce);
            _stateMachine.Update();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_groundSensor)
            {
                if (_characterController == null) _characterController = GetComponent<CharacterController>();
                _groundSensor.DrawGizmos(transform.position, Vector2.down, _characterController.slopeLimit);
            }
        }
#endif


        private GUIStyle _labelStyle;
        private GUIStyle _buttonStyle;

        private void OnGUI()
        {
            if (_labelStyle == null)
            {
                _labelStyle = new GUIStyle();
                _labelStyle.fontSize = 40;
                _labelStyle.normal.textColor = Color.white;
            }
            var velocity = _movementSystem.Velocity;
            var groundState = _movementSystem.GroundState;

            GUILayout.Label($"Velocity: {velocity.x: 000.00;-000.00; 000.00}, {velocity.y: 000.00;-000.00; 000.00}", _labelStyle);
            GUILayout.Label($"GroundState: {groundState}", _labelStyle);

            if (_buttonStyle == null)
            {
                _buttonStyle = new GUIStyle(GUI.skin.button);
                _buttonStyle.fontSize = 40;
            }
            if (GUILayout.Button("Speed 0.1", _buttonStyle)) Time.timeScale = 0.1f;
            if (GUILayout.Button("Speed 0.5", _buttonStyle)) Time.timeScale = 0.5f;
            if (GUILayout.Button("Speed 1", _buttonStyle)) Time.timeScale = 1;
            if (GUILayout.Button("Speed 2", _buttonStyle)) Time.timeScale = 2;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        private bool _showDefaultMovementSettings = false;
        private bool _showGroundSensor = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var playerController = target as PlayerController;
            if (playerController == null) return;

            var movementSettings = playerController.DefaultMovementSettings;
            if (movementSettings != null)
            {
                _showDefaultMovementSettings = UnityEditor.EditorGUILayout.Foldout(_showDefaultMovementSettings, "Movement Settings");
                if (_showDefaultMovementSettings)
                {
                    var serializedObject = new UnityEditor.SerializedObject(movementSettings);
                    var property = serializedObject.GetIterator();
                    property.NextVisible(true); // スクリプトフィールドをスキップ

                    while (property.NextVisible(false))
                    {
                        UnityEditor.EditorGUILayout.PropertyField(property, true);
                    }
                    serializedObject.ApplyModifiedProperties();
                }
            }

            var groundedSensor = playerController.GroundSensor;
            if (groundedSensor != null)
            {
                _showGroundSensor = UnityEditor.EditorGUILayout.Foldout(_showGroundSensor, "Ground Sensor");
                if (_showGroundSensor)
                {
                    var serializedObject = new UnityEditor.SerializedObject(groundedSensor);
                    var property = serializedObject.GetIterator();
                    property.NextVisible(true); // スクリプトフィールドをスキップ

                    while (property.NextVisible(false))
                    {
                        UnityEditor.EditorGUILayout.PropertyField(property, true);
                    }
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
#endif
}