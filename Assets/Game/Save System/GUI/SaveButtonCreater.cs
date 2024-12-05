using System;
using UnityEngine;

namespace Confront.SaveSystem.GUI
{
    public class SaveButtonCreator : MonoBehaviour
    {
        [SerializeField]
        private SaveButton _saveButtonPrefab;
        [SerializeField]
        private Transform _saveButtonParent;

        private void Start()
        {
            var saveDataControllers = SaveDataRepository.SaveDataControllers;
            foreach (var saveDataController in saveDataControllers)
            {
                var saveButton = Instantiate(_saveButtonPrefab, _saveButtonParent);
                saveButton.SaveDataController = saveDataController;
            }
        }
    }
}