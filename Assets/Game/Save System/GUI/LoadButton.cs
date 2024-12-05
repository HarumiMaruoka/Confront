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

        private SaveDataController _saveDataController;
        public SaveDataController SaveDataController
        {
            get => _saveDataController;
            set
            {
                _saveDataController = value;
                var saveDataFile = value.SaveFileData;
                title.text = $"Load {_saveDataController.Key}";
                _description.text = string.IsNullOrEmpty(saveDataFile?.SceneName) ? "None" : saveDataFile?.SceneName;
                _lastPlayed.text = saveDataFile == null ? saveDataFile.LastPlayed.ToString() : new DateTime().ToString();
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SaveDataController.Load);
        }
    }
}