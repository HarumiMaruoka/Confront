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
        private InputActionProperty _action;

        private void Awake() => _action.action.performed += OnPerformed;
        private void OnDestroy() => _action.action.performed -= OnPerformed;
        private void OnEnable() => _action.action.Enable();
        private void OnDisable() => _action.action.Disable();

        public static InputDevice LastInputDevice { get; private set; }

        public static event Action<InputDevice> OnInputDeviceChanged;

        private void Start()
        {
            LastInputDevice = _defaultDevice;
            OnInputDeviceChanged?.Invoke(_defaultDevice);
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            // CallbackContextからControlを取得
            var control = context.control;

            // Controlからデバイスを取得
            var device = control.device;

            if (device is Keyboard || device is Mouse && LastInputDevice != InputDevice.KeyboardMouse)
            {
                LastInputDevice = InputDevice.KeyboardMouse;
                OnInputDeviceChanged?.Invoke(InputDevice.KeyboardMouse);
            }
            else if (device is Gamepad && LastInputDevice != InputDevice.Gamepad)
            {
                LastInputDevice = InputDevice.Gamepad;
                OnInputDeviceChanged?.Invoke(InputDevice.Gamepad);
            }
        }
    }

    public enum InputDevice
    {
        KeyboardMouse,
        Gamepad,
        Any,
    }
}