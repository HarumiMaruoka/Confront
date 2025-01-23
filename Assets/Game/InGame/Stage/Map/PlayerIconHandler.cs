using System;
using UnityEngine;

namespace Confront.Stage.Map
{
    public class PlayerIconHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _playerIconRenderer;

        public static SpriteRenderer PlayerIconRenderer;

        private void OnValidate()
        {
            PlayerIconRenderer = _playerIconRenderer;
        }

        private void Awake()
        {
            PlayerIconRenderer = _playerIconRenderer;
        }
    }
}
