using Confront.SaveSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GameUI
{
    public class GameStartButton : MonoBehaviour
    {
        // 最後にセーブされたデータを読み込む

        private void OnEnable()
        {
            var lastSaveData = GetLastSaveData();
            gameObject.SetActive(lastSaveData != null); // セーブデータが存在しない場合は非表示にする。
        }

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


            SaveDataController lastSaveData = saveDataControllers[0];
            for (int i = 1; saveDataControllers.Length > i; i++)
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
            if (last == null)
            {
                Debug.LogWarning("SaveData is not found.");
                return;
            }
            last.Load();
        }
    }
}