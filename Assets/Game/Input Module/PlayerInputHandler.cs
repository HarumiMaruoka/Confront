﻿using System;
using UnityEngine;

namespace Confront.Input
{
    public static class PlayerInputHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
        }

        private static PlayerInput _playerInput;
        public static PlayerInput PlayerInput => _playerInput;
        public static PlayerInput.InGameInputActions InGameInput => _playerInput.InGameInput;
    }
}