using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Weapon
{
    public static class WeaponManager
    {
        static WeaponManager()
        {
            var handle = Addressables.LoadAssetAsync<WeaponSheet>("WeaponSheet");
            handle.WaitForCompletion();
            WeaponSheet = handle.Result;
            WeaponSheet.Initialize();
        }

        public static WeaponSheet WeaponSheet { get; private set; }
    }
}