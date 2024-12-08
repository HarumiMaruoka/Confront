using System;
using Confront.Utility;
using OdinSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Confront.SaveSystem
{
    public class SaveDataController
    {
        public const int SaveButtonIconWidth = 1920 / 8;
        public const int SaveButtonIconHeight = 1080 / 8;
        public const int SaveButtonIconQuality = 40;

        public static SaveData Loaded { get; private set; }

        public SaveDataController(string key)
        {
            Key = key;
        }
        public readonly string Key;

        public void Save()
        {
            // セーブデータを作成する。
            var saveData = new SaveData();
            var saveFileData = new SaveFileData();
            saveData.SceneName = SceneManager.GetActiveScene().name;
            saveFileData.SceneName = SceneManager.GetActiveScene().name;

            var quality = SaveButtonIconQuality;
            var width = SaveButtonIconWidth;
            var height = SaveButtonIconHeight;

            saveFileData.ScreenShot = Camera.main.CaptureScreenShotJPG(quality, width, height);
            saveFileData.LastPlayed = DateTime.Now;
            foreach (var savable in SavableRegistry.Savables)
            {
                savable.Save(saveData);
            }
            var bytes = SerializationUtility.SerializeValue(saveData, DataFormat.Binary);
            PlayerPrefs.SetString(Key, Convert.ToBase64String(bytes));
            bytes = SerializationUtility.SerializeValue(saveFileData, DataFormat.Binary);
            PlayerPrefs.SetString(Key + "_file", Convert.ToBase64String(bytes));
        }

        public void Load()
        {
            var data = PlayerPrefs.GetString(Key);
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogWarning("SaveData is not found.");
                return;
            }

            var bytes = Convert.FromBase64String(data);
            Loaded = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.Binary);
            // ここでゲームシーンを復元する。
            SceneManager.LoadScene(Loaded.SceneName);
        }

        public SaveFileData SaveFileData
        {
            get
            {
                var data = PlayerPrefs.GetString(Key + "_file");
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }
                var bytes = Convert.FromBase64String(data);
                return SerializationUtility.DeserializeValue<SaveFileData>(bytes, DataFormat.Binary);
            }
        }
    }
}