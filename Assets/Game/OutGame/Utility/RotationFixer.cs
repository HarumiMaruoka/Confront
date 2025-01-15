using System;
using UnityEngine;

namespace Confront.Utility
{
    public class RotationFixer : MonoBehaviour
    {
        private Quaternion _rotation;

        private void Start()
        {
            _rotation = transform.rotation;
        }

        private void Update()
        {
            transform.rotation = _rotation;
        }
    }
}