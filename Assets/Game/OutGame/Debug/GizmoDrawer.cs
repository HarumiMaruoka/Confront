using System;
using UnityEngine;

namespace Confront.Debugger
{
    public class GizmoDrawer : MonoBehaviour
    {
        public static event Action OnDrawGizmosEvent;

        private void OnDrawGizmos()
        {
            OnDrawGizmosEvent?.Invoke();
        }
    }
}