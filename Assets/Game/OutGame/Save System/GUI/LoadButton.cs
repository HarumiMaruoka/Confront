using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.SaveSystem.GUI
{
    [RequireComponent(typeof(Button))]
    public class LoadButton : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI title;
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
                if (_saveDataController == value) return;

                if (_saveDataController != null) _saveDataController.OnSaved -= OnSaved;
                _saveDataController = value;
                _saveDataController.OnSaved += OnSaved;

                OnSaved(value);
            }
        }

        private void OnDestroy()
        {
            if (_saveDataController != null) _saveDataController.OnSaved -= OnSaved;
        }

        private void OnSaved(SaveDataController saveDataController)
        {
            var saveDataFile = saveDataController.SaveFileData;
            title.text = $"Load {saveDataController.Key}";
            _description.text = string.IsNullOrEmpty(saveDataFile?.SceneName) ? "None" : saveDataFile?.SceneName;
            _lastPlayed.text = saveDataFile == null ? "0000/00/00 0:00:00" : saveDataFile.LastPlayed.ToString();

            var width = Camera.main.pixelWidth;
            var height = Camera.main.pixelHeight;
            Texture2D texture2D = new Texture2D(width, height);
            if (saveDataFile != null && ImageConversion.LoadImage(texture2D, saveDataFile.ScreenShot))
            {
                _screenShot.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SaveDataController.Load);
        }
    }
}