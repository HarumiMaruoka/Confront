using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Confront.CameraUtilites
{
    public class CameraShakeHandler : MonoBehaviour
    {
        public static CameraShakeHandler Instance { get; private set; }

        [SerializeField]
        private CinemachineVirtualCamera _camera;

        private CinemachineBasicMultiChannelPerlin _noise;
        private Coroutine shakeCoroutine;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Multiple CameraShakeHandler instances detected.");
            }

            Instance = this;
            _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noise.AmplitudeGain = 0;
            _noise.FrequencyGain = 0;
        }

        public void Shake(float amplitude, float frequency, float duration)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);

            shakeCoroutine = StartCoroutine(ShakeRoutine(amplitude, frequency, duration));
        }

        private IEnumerator ShakeRoutine(float amplitude, float frequency, float duration)
        {
            if (_noise == null)
            {
                Debug.LogWarning("CinemachineBasicMultiChannelPerlin not found.");
                yield break;
            }

            _noise.AmplitudeGain = amplitude;
            _noise.FrequencyGain = frequency;

            yield return new WaitForSeconds(duration);

            _noise.AmplitudeGain = 0;
            _noise.FrequencyGain = 0;
        }
    }
}