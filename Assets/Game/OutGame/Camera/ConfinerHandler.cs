using Cinemachine;
using System;
using UnityEngine;

namespace Confront.CameraUtilites
{
    [DefaultExecutionOrder(-100)]
    public class ConfinerHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineConfiner _confiner;

        private void Awake()
        {
            Confiner = _confiner;
        }

        private static CinemachineConfiner Confiner;

        public static void SetPolygonCollider(PolygonCollider2D polygonCollider)
        {
            Confiner.m_BoundingShape2D = polygonCollider;
        }
    }
}