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
        [SerializeField]
        OverLapSphere _overLapSphereSample = default;

        private void Start()
        {
            _overLapBoxSample.Init(transform);
            _raycastSample.Init(transform);
            _overLapSphereSample.Init(transform);
        }
        private void Update()
        {
            _overLapSphereSample.Enter();
            // Debug.Log($"_overLapBoxSample{_overLapBoxSample.IsHit()}");
            // Debug.Log($"_raycastSample{_raycastSample.IsHit()}");
            Debug.Log($"_overLapSphereSample{_overLapSphereSample.IsHit()}");
        }
        private void OnDrawGizmos()
        {
            _overLapBoxSample.OnDrawGizmos(transform);
            _raycastSample.OnDrawGizmo(transform);
            _overLapSphereSample.OnDrawGizmos(transform);
        }
    }
}