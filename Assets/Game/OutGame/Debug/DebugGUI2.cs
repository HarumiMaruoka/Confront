using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Debugger
{
    public class DebugGUI2 : MonoBehaviour
    {
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
        private TMPro.TextMeshProUGUI _groundedNormal;
        [SerializeField]
        private TMPro.TextMeshProUGUI _groundedNormalAngle;
        [SerializeField]
        private TMPro.TextMeshProUGUI _leftStick;
        [SerializeField]
        private Toggle _canPlayerAttack;
        [SerializeField]
        private Toggle _stateChangeLogging;

        [Header("GameSpeed")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _gameSpeedText;
        [SerializeField]
        private Slider _gameSpeedSlider;
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
    }
}