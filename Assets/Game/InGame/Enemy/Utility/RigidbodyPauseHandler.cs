using Confront.GUI;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public class RigidbodyPauseHandler : IDisposable
    {
        public RigidbodyPauseHandler(Rigidbody rigidbody)
        {
            Rigidbody = rigidbody;
            MenuController.OnOpenedMenu += Pause;
            MenuController.OnClosedMenu += Resume;
        }

        public void Dispose()
        {
            MenuController.OnOpenedMenu -= Pause;
            MenuController.OnClosedMenu -= Resume;
        }

        private readonly Rigidbody Rigidbody;
        private Vector3 SavedVelocity;
        private Vector3 SavedAngularVelocity;
        private bool IsPaused;

        private void Pause()
        {
            if (IsPaused) return;

            // 現在の速度を保存
            SavedVelocity = Rigidbody.linearVelocity;
            SavedAngularVelocity = Rigidbody.angularVelocity;

            // Rigidbodyを停止状態にする
            Rigidbody.isKinematic = true;

            IsPaused = true;
        }

        private void Resume()
        {
            if (!IsPaused) return;
            // Rigidbodyを元に戻す
            Rigidbody.isKinematic = false;
            Rigidbody.linearVelocity = SavedVelocity;
            Rigidbody.angularVelocity = SavedAngularVelocity;
            IsPaused = false;
        }
    }
}