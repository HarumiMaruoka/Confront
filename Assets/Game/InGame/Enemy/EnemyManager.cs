using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Enemy
{
    public static class EnemyManager
    {
        static EnemyManager()
        {
            var handle = Addressables.LoadAssetAsync<EnemySheet>("EnemySheet");
            handle.WaitForCompletion();
            EnemySheet = handle.Result;
            EnemySheet.Initialize();
        }

        public static EnemySheet EnemySheet { get; }
    }
}