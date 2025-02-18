using Confront.Input;
using Confront.Player;
using Confront.Player.Combo;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Debugger
{
    public class DebugGUI : MonoBehaviour
    {
        [SerializeField]
        private Toggle _debugMode;
        [SerializeField]
        private GameObject[] _debugPanels;

        [Header("FPS")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _fpsText;

        [Header("PlayerStates")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _currentState;
        [SerializeField]
        private TMPro.TextMeshProUGUI _velocity;
        [SerializeField]
        private TMPro.TextMeshProUGUI _isGrounded;
        [SerializeField]
        private TMPro.TextMeshProUGUI _isAbyss;
        [SerializeField]
        private TMPro.TextMeshProUGUI _isSteepSlope;
        [SerializeField]
        private TMPro.TextMeshProUGUI _groundNormal;
        [SerializeField]
        private TMPro.TextMeshProUGUI _groundNormalAngle;
        [SerializeField]
        private TMPro.TextMeshProUGUI _leftStick;
        [SerializeField]
        private TMPro.TextMeshProUGUI _attackState;
        [SerializeField]
        private Toggle _canPlayerAttack;
        [SerializeField]
        private Toggle _stateChangeLogging;
        [SerializeField]
        private Toggle _isGodMode;
        [SerializeField]
        private Toggle _isInfiniteJump;

        [Header("GameSpeed")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _gameSpeedText;
        [SerializeField]
        private Slider _gameSpeedSlider;
        [SerializeField]
        private Button _gameSpeed001f;
        [SerializeField]
        private Button _gameSpeed005f;
        [SerializeField]
        private Button _gameSpeed01f;
        [SerializeField]
        private Button _gameSpeed05f;
        [SerializeField]
        private Button _gameSpeed1f;
        [SerializeField]
        private Button _gameSpeed2f;

        [Header("FrameRate")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _frameRateText;
        [SerializeField]
        private Slider _farameRateSlider;
        [SerializeField]
        private Button _frameRate15;
        [SerializeField]
        private Button _frameRate30;
        [SerializeField]
        private Button _frameRate60;
        [SerializeField]
        private Button _frameRate120;
        [SerializeField]
        private Button _frameRateMax;

        private void Start()
        {
            InitializeGameSpeed();
            InitializeFrameRateUI();

            _debugMode.isOn = DebugParams.Instance.DebugMode;
            _canPlayerAttack.isOn = DebugParams.Instance.CanPlayerAttack;
            _stateChangeLogging.isOn = DebugParams.Instance.StateTransitionLogging;
            _isGodMode.isOn = DebugParams.Instance.IsGodMode;
            _isInfiniteJump.isOn = DebugParams.Instance.IsInfiniteJump;

            OnDebugModeChanged(_debugMode.isOn);
            _debugMode.onValueChanged.AddListener(OnDebugModeChanged);
        }

        private void OnDebugModeChanged(bool isOn)
        {
            DebugParams.Instance.DebugMode = isOn;

            foreach (var panel in _debugPanels)
            {
                panel.SetActive(isOn);
            }
        }

        private void Update()
        {
            var player = PlayerController.Instance;
            if (!player) return;

            UpdateFPSView();

            var fps = 1 / Time.deltaTime;
            var currentState = player.StateMachine.CurrentState.GetType().Name;
            var velocity = player.MovementParameters.Velocity;
            var groundSensorResult = player.Sensor.CalculateGroundState(player);
            var isGrounded = groundSensorResult.IsGrounded;
            var isAbyss = groundSensorResult.IsAbyss;
            var isSteepSlope = groundSensorResult.IsSteepSlope;
            var groundedNormal = groundSensorResult.GroundNormal;
            var groundedNormalAngle = Vector3.Angle(Vector3.up, groundSensorResult.GroundNormal);
            var leftStickInput = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
            var leftStick = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
            var playerAttackStateMachine = player.StateMachine.CurrentState as AttackStateMachine;
            var plaeyrAttackState = playerAttackStateMachine?.CurrentNode?.Behaviour?.name;

            _currentState.text = $"Current State: {currentState}";
            _velocity.text = $"Velocity: {velocity}";
            _isGrounded.text = $"Is Grounded: {isGrounded}";
            _isAbyss.text = $"Is Abyss: {isAbyss}";
            _isSteepSlope.text = $"Is Steep Slope: {isSteepSlope}";
            _groundNormal.text = $"Grounded Normal: {groundedNormal}";
            _groundNormalAngle.text = $"Grounded Normal Angle: {groundedNormalAngle.ToString("0.00")}";
            _leftStick.text = $"Left Stick: {leftStick}";
            _attackState.text = $"Attack State: {plaeyrAttackState}";

            DebugParams.Instance.CanPlayerAttack = _canPlayerAttack.isOn;
            DebugParams.Instance.StateTransitionLogging = _stateChangeLogging.isOn;
            DebugParams.Instance.IsGodMode = _isGodMode.isOn;
            DebugParams.Instance.IsInfiniteJump = _isInfiniteJump.isOn;
        }

        private void InitializeGameSpeed()
        {
            _gameSpeedSlider.minValue = 0.01f;
            _gameSpeedSlider.maxValue = 2f;
            _gameSpeedSlider.value = Time.timeScale;

            _gameSpeedSlider.onValueChanged.AddListener(UpdateGameSpeed);
            _gameSpeed001f.onClick.AddListener(() => UpdateGameSpeed(0.01f));
            _gameSpeed005f.onClick.AddListener(() => UpdateGameSpeed(0.05f));
            _gameSpeed01f.onClick.AddListener(() => UpdateGameSpeed(0.1f));
            _gameSpeed05f.onClick.AddListener(() => UpdateGameSpeed(0.5f));
            _gameSpeed1f.onClick.AddListener(() => UpdateGameSpeed(1f));
            _gameSpeed2f.onClick.AddListener(() => UpdateGameSpeed(2f));

            UpdateGameSpeedUI();
        }

        private void UpdateGameSpeed(float gameSpeed)
        {
            Time.timeScale = gameSpeed;
            UpdateGameSpeedUI();
        }

        private void UpdateGameSpeedUI()
        {
            _gameSpeedText.text = $"Game Speed: {Time.timeScale.ToString("0.00")}";
            _gameSpeedSlider.value = Time.timeScale;
        }

        private void InitializeFrameRateUI()
        {
            _farameRateSlider.minValue = 15;
            _farameRateSlider.maxValue = 120;
            _farameRateSlider.value = Application.targetFrameRate;

            _farameRateSlider.onValueChanged.AddListener(UpdateFrameRate);
            _frameRate15.onClick.AddListener(() => UpdateFrameRate(15));
            _frameRate30.onClick.AddListener(() => UpdateFrameRate(30));
            _frameRate60.onClick.AddListener(() => UpdateFrameRate(60));
            _frameRate120.onClick.AddListener(() => UpdateFrameRate(120));
            _frameRateMax.onClick.AddListener(() => UpdateFrameRate(-1));

            UpdateFrameRateUI();
        }

        private void UpdateFrameRate(float farameRate)
        {
            Application.targetFrameRate = (int)farameRate;
            UpdateFrameRateUI();
        }

        private void UpdateFrameRateUI()
        {
            _frameRateText.text = $"Frame Rate: {Application.targetFrameRate}";
            if (Application.targetFrameRate == -1)
            {
                _farameRateSlider.value = _farameRateSlider.maxValue;
            }
            else
            {
                _farameRateSlider.value = Application.targetFrameRate;
            }
        }

        private float _elapsed = 0;
        private float _fpsUpdateInterval = 0.1f;

        private void UpdateFPSView()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed < _fpsUpdateInterval) return;
            var fps = 1 / Time.deltaTime;
            _fpsText.text = $"FPS: {fps.ToString("0.00")}";
            _elapsed = 0;
        }
    }
}