using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Stage
{
    public static class StageManager
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void Initialize()
        //{
        //    var handle = Addressables.LoadAssetsAsync<StageController>("Stages", null);
        //    handle.WaitForCompletion();
        //    _stagePrefabs = handle.Result;
        //    _stagePrefabMap = _stagePrefabs.ToDictionary(stage => stage.name);
        //}

        private static IList<StageController> _stagePrefabs;
        private static Dictionary<string, StageController> _stagePrefabMap;

        private static Dictionary<string, StageController> _loadedStageMap = new Dictionary<string, StageController>(); // 読み込み済みのステージ。
        private static HashSet<string> _loadingStages = new HashSet<string>(); // 隣接している読み込み中のステージ。

        private static StageController _currentStage;
        public static StageController CurrentStage => _currentStage;

        public static async UniTask ChangeStage(string nextStageName, int startPointIndex, Func<UniTask> fadeout = null, Func<UniTask> fadein = null, CancellationToken token = default)
        {
            if (!_loadedStageMap.ContainsKey(nextStageName) && !_loadingStages.Contains(nextStageName))
            {
                // まだ読み込んでいないステージの場合、読み込む。
                await LoadStages(token, nextStageName);
            }
            else if (_loadingStages.Contains(nextStageName))
            {
                // 既に読み込み中のステージがある場合、その読み込みが完了するまで待つ。
                while (_loadingStages.Contains(nextStageName))
                {
                    await UniTask.Yield(token);
                }
            }

            if (fadeout != null) await fadeout();
            ChangeStage(_loadedStageMap[nextStageName], startPointIndex, token);
            if (fadein != null) await fadein();
        }

        /// <summary> ステージ変更時に呼ばれるイベント。第一引数に変更後のステージ、第二引数にスタート地点を渡す。 </summary>
        public static event Action<StageController, Vector2> OnStageChanged;

        private static void ChangeStage(StageController stage, int startPointIndex, CancellationToken token)
        {
            _currentStage.gameObject.SetActive(false);
            _currentStage = stage;
            _currentStage.gameObject.SetActive(true);
            var startPoint = _currentStage.StartPoints[startPointIndex].position;
            OnStageChanged?.Invoke(_currentStage, startPoint);
            LoadStages(token, stage.ConnectedStages).Forget();
        }

        private static async UniTask LoadStages(CancellationToken token, params string[] stageNames)
        {
            foreach (var stageName in stageNames)
            {
                // 既に読み込み済み、または 読み込み中のステージは読み込まない。
                if (_loadedStageMap.ContainsKey(stageName) || _loadingStages.Contains(stageName))
                {
                    continue;
                }

                _loadingStages.Add(stageName);
                var prefab = _stagePrefabMap[stageName];
                var handle = GameObject.InstantiateAsync(prefab);

                while (!handle.isDone)
                {
                    await UniTask.Yield(token);
                }

                _loadingStages.Remove(stageName);
                var stage = handle.Result[0];
                stage.gameObject.SetActive(false);
                _loadedStageMap.Add(stageName, stage);
            }
        }
    }
}