using Confront.GUI;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public class AnimatorPauseHandler : IDisposable
    {
        public AnimatorPauseHandler(Animator animator)
        {
            _animator = animator;
            MenuController.OnOpenedMenu += Pause;
            MenuController.OnClosedMenu += Resume;
        }

        public void Dispose()
        {
            MenuController.OnOpenedMenu -= Pause;
            MenuController.OnClosedMenu -= Resume;
        }

        private readonly Animator _animator;
        private float _savedSpeed;
        private bool _isPaused;

        private void Pause()
        {
            if (_isPaused) return;

            _savedSpeed = _animator.speed;
            _animator.speed = 0;

            _isPaused = true;
        }

        private void Resume()
        {
            if (!_isPaused) return;

            _animator.speed = _savedSpeed;

            _isPaused = false;
        }
    }
}