using Confront.GameUI;
using Confront.Input;
using Confront.Player.Combo;
using Confront.SaveSystem;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Confront.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, ISavable
    {
        // インゲーム制御
        public StateMachine StateMachine;
        // 移動系
        public MovementParameters MovementParameters;
        public Sensor Sensor;
        public DirectionController DirectionController;
        // ステータス系
        public CharacterStats CharacterStats;
        public HealthManager HealthManager;
        // 攻撃系
        public ComboTree AttackComboTree;
        public AttackStateMachine AttackStateMachine;
        // メニュー
        public MenuController MenuController;
        // Unityコンポーネント
        private CharacterController _characterController;
        private Animator _animator;

        public CharacterController CharacterController { get => _characterController ??= GetComponent<CharacterController>(); }
        public Animator Animator { get => _animator ??= GetComponent<Animator>(); }

        private void Start()
        {
            SavableRegistry.Register(this);
            Initialize(SaveDataController.Loaded);
        }

        private bool _isInitialized = false;

        public void Initialize(SaveData saveData = null)
        {
            if (!_isInitialized)
            {
                StateMachine = new StateMachine(this);
                StateMachine.ChangeState<Grounded>();
                DirectionController.Initialize(transform);
                AttackStateMachine = new AttackStateMachine();
                HealthManager = new HealthManager(CharacterStats.MaxHealth, CharacterStats.MaxHealth);
                _isInitialized = true;
                MenuController.OnOpenedMenu += OnOpenedMenu;
                MenuController.OnClosedMenu += OnClosedMenu;
            }
            if (saveData != null && saveData.PlayerData.HasValue)
            {
                var value = saveData.PlayerData.Value;
                transform.position = value.Position;
                transform.rotation = value.Rotation;
                StateMachine.ChangeState(value.PlayerStateType);
                MovementParameters.Velocity = value.Velocity;
                MovementParameters.GrabIntervalTimer = value.GrabIntervalTimer;
                MovementParameters.PassThroughPlatformDisableTimer = value.PassThroughPlatformDisableTimer;
                saveData.PlayerData = null; // 何度もロードしないようにするため
            }
        }

        private void Update()
        {
            if (MenuController.IsOpenedMenu) return;

            StateMachine.Update();
            if (CharacterController.enabled) CharacterController.Move(MovementParameters.Velocity * Time.deltaTime);
            Animator.SetFloat("RunSpeed", Mathf.Abs(MovementParameters.Velocity.x / MovementParameters.MaxSpeed));
            DirectionController.UpdateVelocity(MovementParameters.Velocity);
            MovementParameters.TimerUpdate();

            if (IsJumpable) StateMachine.ChangeState<Jump>();

            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        private void OnDestroy()
        {
            SavableRegistry.Unregister(this);
            if (MenuController)
            {
                MenuController.OnOpenedMenu -= OnOpenedMenu;
                MenuController.OnClosedMenu -= OnClosedMenu;
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

        private void OnOpenedMenu()
        {
            Animator.StartPlayback();
        }

        private void OnClosedMenu()
        {
            Animator.StopPlayback();
        }

        public void Save(SaveData saveData)
        {
            saveData.PlayerData = new PlayerData
            {
                // Transform
                Position = transform.position,
                Rotation = transform.rotation,
                // StateMachine
                PlayerStateType = StateMachine.CurrentState.GetType(),
                // MovementParameters
                Velocity = MovementParameters.Velocity,
                GrabIntervalTimer = MovementParameters.GrabIntervalTimer,
                PassThroughPlatformDisableTimer = MovementParameters.PassThroughPlatformDisableTimer,
                // HealthManager
                Health = HealthManager.CurrentHealth,
                MaxHealth = CharacterStats.MaxHealth,
            };

        }

        private bool IsJumpable
        {
            get
            {
                var leftStick = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
                if (leftStick.sqrMagnitude > 0.1f)
                {
                    var angle = Vector2.Angle(leftStick, Vector2.down);
                    if (PlayerInputHandler.InGameInput.Jump.triggered && angle < 45f)
                    {
                        return false;
                    }
                }

                if (PlayerInputHandler.InGameInput.Jump.triggered) return true;

                return false;
            }
        }
    }

#if UNITY_EDITOR
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
#endif
}