using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Title
{
    public class OptionButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject _optionPanel;

        private void Start() => GetComponent<Button>().onClick.AddListener(() => _optionPanel.SetActive(true));
    }
}