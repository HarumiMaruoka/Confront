using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Confront.Input
{
    public class InputDeviceManager : MonoBehaviour
    {
        [SerializeField]
        private InputDevice _defaultDevice;

        [SerializeField]
        private GameObject[] _keyboardMouseUI;
        [SerializeField]
        private GameObject[] _gamepadUI;

        [SerializeField]
        private InputActionProperty _action;

        private void Awake() => _action.action.performed += OnPerformed;
        private void OnDestroy() => _action.action.performed -= OnPerformed;
        private void OnEnable() => _action.action.Enable();
        private void OnDisable() => _action.action.Disable();

        private InputDevice _lastInput;

        public static event Action<InputDevice> OnInputDeviceChanged;

        private void Start()
        {
            var activeUIElements = _defaultDevice == InputDevice.KeyboardMouse ? _keyboardMouseUI : _gamepadUI;
            var inactiveUIElements = _defaultDevice == InputDevice.KeyboardMouse ? _gamepadUI : _keyboardMouseUI;

            foreach (var ui in activeUIElements) ui.SetActive(true);
            foreach (var ui in inactiveUIElements) ui.SetActive(false);

            _lastInput = _defaultDevice;
            OnInputDeviceChanged?.Invoke(_defaultDevice);
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            // CallbackContextからControlを取得
            var control = context.control;

            // Controlからデバイスを取得
            var device = control.device;
            if (device is Keyboard || device is Mouse)
            {
                UpdateInputDevice(InputDevice.KeyboardMouse, _keyboardMouseUI, _gamepadUI);
            }
            else if (device is Gamepad)
            {
                UpdateInputDevice(InputDevice.Gamepad, _gamepadUI, _keyboardMouseUI);
            }
        }

        private void UpdateInputDevice(InputDevice newDevice, GameObject[] activeUIElements, GameObject[] inactiveUIElements)
        {
            if (_lastInput != newDevice)
            {
                foreach (var ui in activeUIElements) ui.SetActive(true);
                foreach (var ui in inactiveUIElements) ui.SetActive(false);

                _lastInput = newDevice;
                OnInputDeviceChanged?.Invoke(newDevice);
            }
        }
    }

    public enum InputDevice
    {
        KeyboardMouse,
        Gamepad,
    }
}