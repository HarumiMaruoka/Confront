using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.ActionItem
{
    public static class ActionItemManager
    {
        static ActionItemManager()
        {
            var handle = Addressables.LoadAssetAsync<ActionItemSheet>("ActionItemSheet");
            handle.WaitForCompletion();
            ActionItemSheet = handle.Result;
            ActionItemSheet.Initialize();
        }

        public static ActionItemSheet ActionItemSheet { get; private set; }
    }
}