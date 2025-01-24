using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Confront.Audio
{
    public static class AudioManager
    {
        private static BGMPlayer _bgmPlayer = new BGMPlayer();
        private static SEPlayer _sePlayer = new SEPlayer();

        public static VolumeParameters VolumeParameters { get; } = new VolumeParameters();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            VolumeParameters.OnBgmVolumeChanged += _bgmPlayer.ChangeVolume;
        }

        public static void PlayBGM(AudioClip clip, float duration = 1f)
        {
            _bgmPlayer.Play(clip, duration);
        }

        public static void PlaySE(Vector3 position, AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, position, AudioManager.VolumeParameters.SeVolume);
        }
    }
}