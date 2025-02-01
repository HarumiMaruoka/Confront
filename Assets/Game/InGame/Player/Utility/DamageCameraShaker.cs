using Confront.CameraUtilites;
using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "DamageCameraShaker", menuName = "ConfrontSO/Player/Utility/DamageCameraShaker")]
    public class DamageCameraShaker : ScriptableObject
    {
        [Header("Damage Camera Shake")]
        public float MiniDamageCameraShakePowerAmplitude = 0.5f;
        public float MiniDamageCameraShakeFrequency = 0.5f;
        public float MiniDamageCameraShakeDuration = 0.5f;
        [Space]
        public float SmallDamageCameraShakePowerAmplitude = 0.5f;
        public float SmallDamageCameraShakeFrequency = 0.5f;
        public float SmallDamageCameraShakeDuration = 0.5f;
        [Space]
        public float BigDamageCameraShakePowerAmplitude = 0.5f;
        public float BigDamageCameraShakeFrequency = 0.5f;
        public float BigDamageCameraShakeDuration = 0.5f;

        public void MiniDamageCameraShake()
        {
            ShakeCamera(MiniDamageCameraShakePowerAmplitude, MiniDamageCameraShakeFrequency, MiniDamageCameraShakeDuration);
        }

        public void SmallDamageCameraShake()
        {
            ShakeCamera(SmallDamageCameraShakePowerAmplitude, SmallDamageCameraShakeFrequency, SmallDamageCameraShakeDuration);
        }

        public void BigDamageCameraShake()
        {
            ShakeCamera(BigDamageCameraShakePowerAmplitude, BigDamageCameraShakeFrequency, BigDamageCameraShakeDuration);
        }

        private static void ShakeCamera(float powerAmplitude, float frequency, float duration)
        {
            if (CameraShakeHandler.Instance == null)
            {
                Debug.LogWarning("CameraShakeHandler not found.");
                return;
            }

            CameraShakeHandler.Instance.Shake(powerAmplitude, frequency, duration);
        }
    }
}