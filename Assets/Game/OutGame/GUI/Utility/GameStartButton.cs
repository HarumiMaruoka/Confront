using Confront.SaveSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Confront.GameUI
{
    // 最後にセーブされたデータを読み込む。
    // セーブデータが存在しない場合は、NewGameを開始する。
    public class GameStartButton : MonoBehaviour
    {
        [SerializeField]
        private string _defaultSceneName = "Game Main";

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        }

        private SaveDataController GetLastSaveData()
        {
            SaveDataController[] saveDataControllers = SaveDataRepository.SaveDataControllers;
            if (saveDataControllers == null || saveDataControllers.Length == 0)
            {
                Debug.LogWarning("SaveData is not found.");
                return null;
            }


            SaveDataController lastSaveData = null;
            for (int i = 0; saveDataControllers.Length > i; i++)
            {
                if (saveDataControllers[i] == null || saveDataControllers[i].SaveFileData == null) continue;

                if (saveDataControllers[i].SaveFileData.LastPlayed > lastSaveData.SaveFileData.LastPlayed)
                {
                    lastSaveData = saveDataControllers[i];
                }
            }

            return lastSaveData;
        }

        private void OnButtonClicked()
        {
            var last = GetLastSaveData();
            if (last != null)
            {
                last.Load();
                return;
            }
            else
            {
                SceneManager.LoadScene(_defaultSceneName);
            }
        }
    }
}