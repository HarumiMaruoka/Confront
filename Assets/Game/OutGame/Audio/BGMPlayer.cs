using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Confront.Audio
{
    public class BGMPlayer
    {
        private AudioSource _audioSource;

        public AudioSource AudioSource
        {
            get
            {
                if (Application.isPlaying && _audioSource == null)
                {
                    _audioSource = new GameObject("BGMPlayer").AddComponent<AudioSource>();
                    GameObject.DontDestroyOnLoad(_audioSource.gameObject);
                    _audioSource.volume = AudioManager.VolumeParameters.BgmVolume;
                    _audioSource.loop = true;
                }
                return _audioSource;
            }
        }

        private CancellationTokenSource _cancellationTokenSource;
        private bool isFadingIn = false;

        public async void Play(AudioClip clip, float duration)
        {
            if (clip == AudioSource.clip) return;
            if (clip == null) return;

            // 前回のフェードアウト/フェードイン処理をキャンセルする
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // フェードアウト
            var startVolume = AudioSource.volume;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                var targetVolume = 0f;
                if (token.IsCancellationRequested) return;
                if (!AudioSource) return;
                await UniTask.Yield();
                if (AudioSource) AudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            }

            // クリップ差し替え
            AudioSource.clip = clip;
            AudioSource.Play();

            // フェードイン
            isFadingIn = true;
            startVolume = AudioSource.volume;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                var targetVolume = AudioManager.VolumeParameters.BgmVolume;
                if (token.IsCancellationRequested)
                {
                    isFadingIn = false;
                    return;
                }
                if (!AudioSource) return;
                AudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
                await UniTask.Yield();
            }
            isFadingIn = false;
        }

        public async UniTask Stop(float duration)
        {
            // 前回のフェードアウト/フェードイン処理をキャンセルする
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // フェードアウト
            var startVolume = AudioSource.volume;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                var targetVolume = 0f;
                if (token.IsCancellationRequested) return;
                if (!AudioSource) return;
                await UniTask.Yield();
                AudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            }
            AudioSource.Stop();
        }

        public void ChangeVolume(float volume)
        {
            if (isFadingIn) return;
            AudioSource.volume = volume;
        }
    }
}