using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class OtherTest : MonoBehaviour
    {
        [SerializeField]
        OverLapBox _overLapBoxSample = default;
        [SerializeField]
        Raycast _raycastSample = default;

        private void Start()
        {
            _overLapBoxSample.Init(transform);
            _raycastSample.Init(transform);
        }
        private void Update()
        {
            Debug.Log($"_overLapBoxSample{_overLapBoxSample.IsHit()}");
            Debug.Log($"_raycastSample{_raycastSample.GetResult()}");
        }
        private void OnDrawGizmos()
        {
            _overLapBoxSample.OnDrawGizmos(transform);
            _raycastSample.OnDrawGizmo(transform);
        }
    }
}