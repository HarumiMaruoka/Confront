using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Confront.Stage
{
    public static class StageManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            LoadStagePrefabs();
            SceneManager.sceneLoaded += Reset;
        }

        private static void Reset(Scene arg0, LoadSceneMode arg1)
        {
            _createdStages.Clear();
        }

        private static void LoadStagePrefabs()
        {
            _stagePrefabs = Resources.LoadAll<StageController>("StagePrefabs");
            _stagePrefabMap = _stagePrefabs.ToDictionary(stage => stage.gameObject.name);
            _stageNames = new List<string>(_stagePrefabs.Length);
            for (int i = 0; i < _stagePrefabs.Length; i++)
            {
                _stageNames.Add(_stagePrefabs[i].gameObject.name);
            }
        }

        private static StageController[] _stagePrefabs;
        private static Dictionary<string, StageController> _stagePrefabMap;

        private static Dictionary<string, StageController> _createdStages = new Dictionary<string, StageController>(); // 生成済みのステージ。

        public static StageController CurrentStage;
        private static bool _isChangingStage = false;
        public static StageController[] Stages => _stagePrefabs;
        private static List<string> _stageNames = null;
        public static List<string> StageNames => _stageNames;

        public static string CurrentStageName => CurrentStage.gameObject.name.Replace("(Clone)", "");

        // プレイヤーを指定したステージの指定したスタート地点に移動させる。
        public static async UniTask ChangeStage(string nextStageName, int startPointIndex, Func<UniTask> fadeout = null, Func<UniTask> fadein = null)
        {
            if (_isChangingStage) return;
            _isChangingStage = true;

            if (fadeout != null) await fadeout();

            for (float t = 0; t < 0.1f; t += Time.deltaTime)
            {
                await UniTask.Yield();
            }

            try
            {
                if (!_createdStages.ContainsKey(nextStageName))
                {
                    _createdStages[nextStageName] = GameObject.Instantiate(_stagePrefabMap[nextStageName]);
                }

                if (startPointIndex < 0 || startPointIndex >= _createdStages[nextStageName].StartPoints.Length)
                {
                    Debug.LogError($"スタート地点「{startPointIndex}」が見つかりません。");
                    return;
                }

                ChangeStage(_createdStages[nextStageName], startPointIndex);
            }

            catch (KeyNotFoundException)
            {
                Debug.LogError($"ステージ「{nextStageName}」が見つかりません。");
            }

            _isChangingStage = false;

            if (fadein != null) await fadein();
        }

        // 指定したステージに切り替える。
        public static async UniTask ChangeStage(string nextStageName, Func<UniTask> fadeout = null, Func<UniTask> fadein = null)
        {
            if (_isChangingStage) return;
            _isChangingStage = true;

            if (fadeout != null) await fadeout();
            try
            {
                if (!_createdStages.ContainsKey(nextStageName))
                {
                    _createdStages.Add(nextStageName, GameObject.Instantiate(_stagePrefabMap[nextStageName]));
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

        public static event Action<StageController> OnStageChanged;

        private static void ChangeStage(StageController stage, int startPointIndex)
        {
            if (CurrentStage) 
                CurrentStage.gameObject.SetActive(false);
            CurrentStage = stage;
            CurrentStage.gameObject.SetActive(true);

            var player = PlayerController.Instance;

            var startPoint = CurrentStage.StartPoints[startPointIndex].position;
            if (player != null)
            {
                var prevEnable = player.CharacterController.enabled;
                player.DisablePlatformCollisionRequest = true;
                player.CharacterController.enabled = false;
                player.transform.position = startPoint;
                player.CharacterController.enabled = prevEnable;
            }

            OnStageChanged?.Invoke(CurrentStage);
        }

        private static void ChangeStage(StageController stage)
        {
            if (CurrentStage) CurrentStage.gameObject.SetActive(false);
            CurrentStage = stage;
            CurrentStage.gameObject.SetActive(true);

            OnStageChanged?.Invoke(CurrentStage);
        }

        public static void Reset()
        {
            _createdStages.Clear();
            CurrentStage = null;
        }
    }
}