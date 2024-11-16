using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "ClimbAnimation", menuName = "Confront/Player/Animation/ClimbAnimation")]
    public class ClimbAnimation : ScriptableObject
    {
        public ClimbAnimationParameters[] GrabAnimationParameters;

        public async UniTask PlayAsync(PlayerController player)
        {
            var startPosition = player.transform.position;
            var nextPosition = player.transform.position;
            var duration = 0f;

            for (int i = 0; i < GrabAnimationParameters.Length; i++)
            {
                startPosition = nextPosition;
                nextPosition = player.transform.position + AdjustXForDirection(player, GrabAnimationParameters[i].NextPosition);
                duration = GrabAnimationParameters[i].Duration;

                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    player.transform.position = Vector2.Lerp(startPosition, nextPosition, t / duration);
                    await UniTask.Yield();
                }

                player.transform.position = nextPosition;
                await UniTask.Yield();
            }
        }

        // プレイヤーの向きに応じてX成分を反転する。
        public Vector3 AdjustXForDirection(PlayerController player, Vector3 target)
        {
            return new Vector3(player.DirectionController.CurrentDirection == Direction.Right ? target.x : -target.x, target.y, target.z);
        }
    }

    [Serializable]
    public struct ClimbAnimationParameters
    {
        public Vector2 NextPosition;
        public float Duration;
    }
}