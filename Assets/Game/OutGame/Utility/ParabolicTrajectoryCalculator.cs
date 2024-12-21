using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Confront.Utility
{
    // 斜方投射の軌跡を計算するクラス
    public static class ParabolicTrajectoryCalculator
    {
        public static async UniTask SimulateTrajectoryAsync(this MonoBehaviour behaviour, float initialSpeed, float angle, float gravity, LayerMask layerMask, CancellationToken token = default)
        {
            var startPosition = behaviour.transform.position;
            var elapsedTime = 0f;
            angle = angle * Mathf.Deg2Rad; // 度をラジアンに変換

            while (!token.IsCancellationRequested && behaviour)
            {
                var previousPosition = behaviour.transform.position;

                elapsedTime += Time.deltaTime;
                var x = initialSpeed * Mathf.Cos(angle) * elapsedTime;
                var y = initialSpeed * Mathf.Sin(angle) * elapsedTime - 0.5f * gravity * elapsedTime * elapsedTime;
                behaviour.transform.position = startPosition + new Vector3(x, y, 0);

                var nextPosition = behaviour.transform.position;

                if (Physics.Linecast(previousPosition, nextPosition, out RaycastHit hitInfo, layerMask))
                {
                    behaviour.transform.position = hitInfo.point;
                    break;
                }

                await UniTask.Yield(token);
            }
        }
    }
}