using System;
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

        private static Camera _camera;

        public static void PlaySE(AudioClip clip)
        {
            if (!_camera) _camera = Camera.main;

            AudioSource.PlayClipAtPoint(clip, _camera.transform.position, AudioManager.VolumeParameters.SeVolume);
        }

        public static void PlaySE(AudioClip clip, Vector3 position)
        {
            _sePlayer.Play(clip);
        }
    }
}