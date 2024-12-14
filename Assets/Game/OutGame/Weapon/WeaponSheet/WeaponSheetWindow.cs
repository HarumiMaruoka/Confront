#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponSheetWindow : NexEditor.GameDataSheet.SheetWindowBase<WeaponData, WeaponSheet, WeaponSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/WeaponSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(WeaponSheetWindow)).Show();
        }
    }
}
#endif