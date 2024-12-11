using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Debugger
{
    [CreateAssetMenu(fileName = "DebugParams", menuName = "ConfrontSO/Debug/DebugParams")]
    public class DebugParams : ScriptableObject
    {
        private static DebugParams _instance;
        public static DebugParams Instance
        {
            get
            {
                if (_instance == null)
                {
                    var handle = Addressables.LoadAssetAsync<DebugParams>("DebugParams");
                    handle.WaitForCompletion();
                    _instance = handle.Result;
                }
                return _instance;
            }
        }

        public bool CanPlayerAttack = true;
    }
}