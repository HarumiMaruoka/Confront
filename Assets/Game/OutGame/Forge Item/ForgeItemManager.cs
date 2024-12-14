using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.ForgeItem
{
    public static class ForgeItemManager
    {
        static ForgeItemManager()
        {
            var handle = Addressables.LoadAssetAsync<ForgeItemSheet>("ForgeItemSheet");
            handle.WaitForCompletion();
            ForgeItemSheet = handle.Result;

            ForgeItemSheet.Initialize();
        }

        public readonly static ForgeItemSheet ForgeItemSheet;
    }
}