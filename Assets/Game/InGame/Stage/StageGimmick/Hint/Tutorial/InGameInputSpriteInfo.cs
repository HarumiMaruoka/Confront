using Confront.Input;
using System;
using UnityEngine;

namespace Confront.GameUI
{
    [CreateAssetMenu(fileName = "InGameInputSpriteInfo", menuName = "ConfrontSO/GUI/InGameInputSpriteInfo")]
    public class InGameInputSpriteInfo : ScriptableObject
    {
        private static InGameInputSpriteInfo _instance;
        public static InGameInputSpriteInfo Instance
        {
            get
            {
                if (!_instance) _instance = Resources.Load<InGameInputSpriteInfo>("UI/InGameInputSpriteInfo");
                return _instance;
            }
        }

        [Header("Keybord Mouse")]
        public string KeyboardMouse_AttackX;
        public string KeyboardMouse_AttackY;
        public string KeyboardMouse_Jump;
        public string KeyboardMouse_Move;
        public string KeyboardMouse_Item;
        public string KeyboardMouse_Dodge;

        [Header("XBox")]
        public string XBox_AttackX;
        public string XBox_AttackY;
        public string XBox_Jump;
        public string XBox_Move;
        public string XBox_Item;
        public string XBox_Dodge;

        [Header("DualSense")]
        public string DualSense_AttackX;
        public string DualSense_AttackY;
        public string DualSense_Jump;
        public string DualSense_Move;
        public string DualSense_Item;
        public string DualSense_Dodge;

        public enum InputAction
        {
            AttackX,
            AttackY,
            Jump,
            Move,
            Item,
            Dodge
        }

        public string GetSprite(InputAction inputAction)
        {
            InputDevice inputType = InputDeviceManager.LastInputDevice;

            switch (inputAction)
            {
                case InputAction.AttackX: return GetAttackX(inputType);
                case InputAction.AttackY: return GetAttackY(inputType);
                case InputAction.Jump: return GetJump(inputType);
                case InputAction.Move: return GetMove(inputType);
                case InputAction.Item: return GetItem(inputType);
                case InputAction.Dodge: return GetDodge(inputType);
                default: Debug.LogWarning("InputAction not found"); return null;
            }
        }

        public string GetAttackX(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_AttackX}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_AttackX}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_AttackX}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }

        public string GetAttackY(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_AttackY}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_AttackY}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_AttackY}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }

        public string GetJump(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_Jump}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_Jump}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_Jump}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }

        public string GetMove(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_Move}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_Move}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_Move}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }

        public string GetItem(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_Item}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_Item}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_Item}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }

        public string GetDodge(InputDevice inputType)
        {
            switch (inputType)
            {
                case InputDevice.KeyboardMouse: return $"<sprite name=\"{KeyboardMouse_Dodge}\">";
                case InputDevice.XBox: return $"<sprite name=\"{XBox_Dodge}\">";
                case InputDevice.DualSense: return $"<sprite name=\"{DualSense_Dodge}\">";
                default: Debug.LogWarning("InputType not found"); return null;
            }
        }
    }
}