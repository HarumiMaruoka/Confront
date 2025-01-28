using Confront.Input;
using System;
using UnityEngine;

namespace Confront.GameUI
{
    public class DeviceSpecificUI : MonoBehaviour
    {
        [SerializeField]
        private InputDevice _device;

        protected virtual void Start()
        {
            InputDeviceManager.OnInputDeviceChanged += OnInputDeviceChanged;
            OnInputDeviceChanged(InputDeviceManager.LastInputDevice);
        }

        private void OnDestroy()
        {
            InputDeviceManager.OnInputDeviceChanged -= OnInputDeviceChanged;
        }

        private void OnInputDeviceChanged(InputDevice device)
        {
            if (_device == InputDevice.Any) return;
            gameObject.SetActive(device == _device);
        }
    }
}