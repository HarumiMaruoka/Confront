using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Title
{
    [RequireComponent(typeof(Button))]
    public class LoadButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject _loadPanel;

        private void Start() => GetComponent<Button>().onClick.AddListener(() => _loadPanel.SetActive(true));
    }
}