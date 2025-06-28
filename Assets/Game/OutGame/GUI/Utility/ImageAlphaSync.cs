using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI.Utility
{
    [ExecuteInEditMode]
    public class ImageAlphaSync : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private CanvasGroup _canvasGroup;


        private void Update()
        {
            if (_image == null || _canvasGroup == null) return;

            _canvasGroup.alpha = _image.color.a;
        }
    }
}