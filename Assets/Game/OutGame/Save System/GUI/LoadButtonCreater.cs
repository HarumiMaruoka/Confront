using System;
using UnityEngine;

namespace Confront.SaveSystem.GUI
{
    public class LoadButtonCreator : MonoBehaviour
    {
        [SerializeField]
        private LoadButton _loadButtonPrefab;
        [SerializeField]
        private Transform _parent;

        private void Start()
        {
            var saveDataControllers = SaveDataRepository.SaveDataControllers;
            foreach (var saveDataController in saveDataControllers)
            {
                var loadButton = Instantiate(_loadButtonPrefab, _parent);
                loadButton.SaveDataController = saveDataController;
            }
        }
    }
}