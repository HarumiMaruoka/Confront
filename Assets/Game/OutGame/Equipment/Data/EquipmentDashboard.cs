using System;
using UnityEngine;
using NexEditor.ScriptableDashboard;
using UnityEngine.AddressableAssets;

namespace Confront.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentDashboard", menuName = "Scriptable Dashboard/EquipmentDashboard")]
    public class EquipmentDashboard : ScriptableDashboard<EquipmentData>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeDashboard()
        {
            var dashboard = Addressables.LoadAssetAsync<EquipmentDashboard>("EquipmentDashboard").WaitForCompletion();
            if (dashboard == null)
            {
                Debug.LogError("Failed to load EquipmentDashboard. Please ensure it is correctly set up in Addressables.");
                return;
            }

            foreach (var data in dashboard)
            {
                data.Initialize();
            }
        }
    }
}