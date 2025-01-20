using Confront.GameUI;
using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Stage
{
    public class StageController : MonoBehaviour
    {
        public Transform[] StartPoints;
        public StageTransitionData[] StageTransitionData;

        private string[] _connectedStages;
        public string[] ConnectedStages => _connectedStages ??= Array.ConvertAll(StageTransitionData, x => x.NextStageName);

        private void OnValidate()
        {
            _connectedStages = null;
        }

        private void Update()
        {
            foreach (var data in StageTransitionData)
            {
                if (data.IsPlayerWithinBounds())
                {
                    StageManager.ChangeStage(
                        data.NextStageName,
                        data.StartPointIndex,
                        ScreenFader.Instance.FadeOut,
                        ScreenFader.Instance.FadeIn,
                        this.GetCancellationTokenOnDestroy()).Forget();
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var data in StageTransitionData)
            {
                data.DrawGizmos();
            }
        }
    }

    [Serializable]
    public class StageTransitionData
    {
        public string NextStageName => NextStageType.ToString() + StageNumber.ToString();
        public StageType NextStageType;
        public int StageNumber;
        public int StartPointIndex;
        public Vector3 TransitionPoint;
        public Vector3 HitBoxHalfSize;

        private PlayerController PlayerController => PlayerController.Instance;

        public bool IsPlayerWithinBounds()
        {
            var position = PlayerController.transform.position;

            return position.x >= TransitionPoint.x - HitBoxHalfSize.x && position.x <= TransitionPoint.x + HitBoxHalfSize.x
                && position.y >= TransitionPoint.y - HitBoxHalfSize.y && position.y <= TransitionPoint.y + HitBoxHalfSize.y;
        }

        public void DrawGizmos()
        {
            Gizmos.color = IsPlayerWithinBounds() ? Color.red : Color.green;
            Gizmos.DrawWireCube(TransitionPoint, HitBoxHalfSize * 2);
        }
    }
}