using System;
using System.Collections.Generic;
using Confront.ActionItem;
using Confront.ForgeItem;
using Confront.GameUI;
using Confront.Stage;
using Confront.Utility;
using Confront.Weapon;
using Cysharp.Threading.Tasks;
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

        public static SaveData Loaded;

        public SaveDataController(string key)
        {
            Key = key;
        }
        public readonly string Key;

        public Action<SaveDataController> OnSaved;

        public void Save()
        {
            // シリアライズする際に参照するUnityオブジェクトをリストに追加する。
            List<UnityEngine.Object> unityReferences = new List<UnityEngine.Object>();
            unityReferences.AddRange(ActionItemManager.ActionItemSheet);
            unityReferences.AddRange(ForgeItemManager.ForgeItemSheet);
            unityReferences.AddRange(WeaponManager.WeaponSheet);

            // セーブデータを作成する。
            var saveData = new SaveData();
            var saveFileData = new SaveFileData();
            saveData.SceneName = SceneManager.GetActiveScene().name;
            saveData.StageName = StageManager.CurrentStageName;
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
            var bytes = SerializationUtility.SerializeValue(saveData, DataFormat.Binary, out unityReferences);
            PlayerPrefs.SetString(Key, Convert.ToBase64String(bytes));
            bytes = SerializationUtility.SerializeValue(saveFileData, DataFormat.Binary);
            PlayerPrefs.SetString(Key + "_file", Convert.ToBase64String(bytes));

            OnSaved?.Invoke(this);
        }

        public async void Load()
        {
            // デシリアライズする際に参照するUnityオブジェクトをリストに追加する。
            List<UnityEngine.Object> unityReferences = new List<UnityEngine.Object>();
            unityReferences.AddRange(ActionItemManager.ActionItemSheet);
            unityReferences.AddRange(ForgeItemManager.ForgeItemSheet);
            unityReferences.AddRange(WeaponManager.WeaponSheet);

            var data = PlayerPrefs.GetString(Key);
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogWarning("SaveData is not found.");
                return;
            }

            var bytes = Convert.FromBase64String(data);
            Loaded = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.Binary, unityReferences);
            // ここでゲームシーンを復元する。
            SceneManager.LoadScene(Loaded.SceneName);
            StageManager.Reset();
            await UniTask.Yield();
            // ステージの復元処理を行う。
            StageManager.ChangeStage(Loaded.StageName, fadein: ScreenFader.Instance.FadeIn).Forget();
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