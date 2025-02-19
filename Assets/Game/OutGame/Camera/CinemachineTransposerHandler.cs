using Cinemachine;
using System;
using UnityEngine;

namespace Confront.CameraUtilites
{
    [DefaultExecutionOrder(-100)]
    public class CinemachineTransposerHandler : MonoBehaviour
    {
        public static CinemachineTransposerHandler Instance { get; private set; }

        [SerializeField]
        private CinemachineVirtualCamera _camera;
        private CinemachineFramingTransposer _transposer;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Multiple CameraShakeHandler instances detected.");
            }

            Instance = this;
            _transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void SetCameraDistance(float distance)
        {
            if (_transposer == null)
            {
                Debug.LogWarning("CinemachineTransposer not found.");
                return;
            }
            _transposer.m_CameraDistance = distance;
        }
    }
}