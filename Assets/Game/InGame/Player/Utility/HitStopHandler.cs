using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Confront.Player
{
    public class HitStopHandler
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            Time.timeScale = 1;
        }

        public async static void Stop(float duration)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Time.timeScale = 0;
            float startTime = Time.realtimeSinceStartup;
            float endTime = startTime + duration;

            try
            {
                while (Time.realtimeSinceStartup < endTime)
                {
                    float elapsed = Time.realtimeSinceStartup - startTime;
                    Time.timeScale = Mathf.Lerp(0, 1, elapsed / duration);
                    await UniTask.Yield(PlayerLoopTiming.TimeUpdate, token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Time.timeScale = 1;
        }
    }
}