using System;
using UnityEngine;

namespace Confront.Debugger
{
    public class DebugInputHandler : MonoBehaviour
    {
        private void Update()
        {
            HandleDebugInput();
        }

        private static void HandleDebugInput()
        {
#if UNITY_EDITOR
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
                var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                clearMethod?.Invoke(null, null);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.V))
            {
                UnityEditor.EditorApplication.isPaused = !UnityEditor.EditorApplication.isPaused;
            }
#endif
        }
    }
}