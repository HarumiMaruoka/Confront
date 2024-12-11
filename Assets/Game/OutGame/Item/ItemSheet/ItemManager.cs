using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Item
{
    public static class ItemManager
    {
        static ItemManager()
        {
            var handle = Addressables.LoadAssetAsync<ItemSheet>("ItemSheet");
            handle.WaitForCompletion();
            ItemSheet = handle.Result;
            ItemSheet.Initialize();
        }

        public static ItemSheet ItemSheet { get; private set; }
    }
}