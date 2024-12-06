using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.SaveSystem.GUI
{
    [RequireComponent(typeof(Button))]
    public class SaveButton : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _title;
        [SerializeField]
        private TMPro.TextMeshProUGUI _description;
        [SerializeField]
        private TMPro.TextMeshProUGUI _lastPlayed;
        [SerializeField]
        private Image _screenShot;

        private SaveDataController _saveDataController;
        public SaveDataController SaveDataController
        {
            get => _saveDataController;
            set
            {
                _saveDataController = value;
                UpdateView(value.SaveFileData);
            }
        }

        private void UpdateView(SaveFileData saveFileData)
        {
            _title.text = $"Save {_saveDataController.Key}";
            _description.text = string.IsNullOrEmpty(saveFileData?.SceneName) ? "None" : saveFileData?.SceneName;
            _lastPlayed.text = saveFileData == null ? "0000/00/00 0:00:00" : saveFileData.LastPlayed.ToString();

            var width = Camera.main.pixelWidth;
            var height = Camera.main.pixelHeight;
            Texture2D texture2D = new Texture2D(width, height);
            if (saveFileData != null && ImageConversion.LoadImage(texture2D, saveFileData.ScreenShot))
            {
                _screenShot.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Save);
        }

        private void Save()
        {
            SaveDataController.Save();
            UpdateView(SaveDataController.SaveFileData);
        }
    }
}