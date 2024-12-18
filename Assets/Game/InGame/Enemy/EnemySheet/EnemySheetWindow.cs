#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.Enemy
{
    public class EnemySheetWindow : NexEditor.GameDataSheet.SheetWindowBase<EnemyData, EnemySheet, EnemySheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/EnemySheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(EnemySheetWindow)).Show();
        }
    }
}
#endif