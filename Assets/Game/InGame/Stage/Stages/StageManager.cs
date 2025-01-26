using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Confront.Stage
{
    public static class StageManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            LoadStagePrefabs();
        }

        private static void LoadStagePrefabs()
        {
            _stagePrefabs = Resources.LoadAll<StageController>("StagePrefabs");
            _stagePrefabMap = _stagePrefabs.ToDictionary(stage => stage.gameObject.name);
        }

        private static IList<StageController> _stagePrefabs;
        private static Dictionary<string, StageController> _stagePrefabMap;

        private static Dictionary<string, StageController> _createdStages = new Dictionary<string, StageController>(); // 生成済みのステージ。

        public static StageController CurrentStage;
        private static bool _isChangingStage = false;

        public static string CurrentStageName => CurrentStage.gameObject.name.Replace("(Clone)", "");

        public static async UniTask ChangeStage(string nextStageName, int startPointIndex, Func<UniTask> fadeout = null, Func<UniTask> fadein = null)
        {
            if (_isChangingStage) return;
            _isChangingStage = true;

            if (fadeout != null) await fadeout();
            try
            {
                if (!_createdStages.ContainsKey(nextStageName))
                {
                    _createdStages[nextStageName] = GameObject.Instantiate(_stagePrefabMap[nextStageName]);
                }

                ChangeStage(_createdStages[nextStageName], startPointIndex);
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError($"ステージ「{nextStageName}」が見つかりません。");
            }

            for (float t = 0; t < 0.1f; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
            _isChangingStage = false;

            if (fadein != null) await fadein();
        }

        public static async UniTask ChangeStage(string nextStageName, Func<UniTask> fadeout = null, Func<UniTask> fadein = null)
        {
            if (_isChangingStage) return;
            _isChangingStage = true;

            if (fadeout != null) await fadeout();
            try
            {
                if (!_createdStages.ContainsKey(nextStageName))
                {
                    _createdStages[nextStageName] = GameObject.Instantiate(_stagePrefabMap[nextStageName]);
                }

                ChangeStage(_createdStages[nextStageName]);
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError($"ステージ「{nextStageName}」が見つかりません。");
            }

            for (float t = 0; t < 0.1f; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }
            _isChangingStage = false;

            if (fadein != null) await fadein();
        }

        /// <summary> ステージ変更時に呼ばれるイベント。第一引数に変更後のステージ、第二引数にスタート地点を渡す。 </summary>
        public static event Action<StageController, Vector2?> OnStageChanged;

        private static void ChangeStage(StageController stage, int startPointIndex)
        {
            if (CurrentStage) CurrentStage.gameObject.SetActive(false);
            CurrentStage = stage;
            CurrentStage.gameObject.SetActive(true);
            var startPoint = CurrentStage.StartPoints[startPointIndex].position;
            var player = PlayerController.Instance;
            if (player != null)
            {
                var prevEnable = player.CharacterController.enabled;
                player.CharacterController.enabled = false; player.transform.position = startPoint;
                player.CharacterController.enabled = prevEnable;
            }
            OnStageChanged?.Invoke(CurrentStage, startPoint);
        }

        private static void ChangeStage(StageController stage)
        {
            if (CurrentStage) CurrentStage.gameObject.SetActive(false);
            CurrentStage = stage;
            CurrentStage.gameObject.SetActive(true);
            OnStageChanged?.Invoke(CurrentStage, null);
        }

        public static void Reset()
        {
            _createdStages.Clear();
            CurrentStage = null;
        }
    }
}