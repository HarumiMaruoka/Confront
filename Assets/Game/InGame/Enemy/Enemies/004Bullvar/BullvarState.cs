using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    public abstract class BullvarState : ScriptableObject
    {
        public abstract string AnimationName { get; }
        public virtual void Enter(PlayerController player, BullvarController bullvar) { }
        public virtual void Execute(PlayerController player, BullvarController bullvar) { }
        public virtual void Exit(PlayerController player, BullvarController bullvar) { }
        public virtual void DrawGizmos(PlayerController player, BullvarController bullvar) { }
    }

#if UNITY_EDITOR
    public static class ScriptableObjectCreator
    {
        [UnityEditor.MenuItem("Assets/Create/ConfrontSO/Enemy/Bullvar/Create All BullvarStates")]
        public static void CreateAllScriptableObjects()
        {
            // 保存先のフォルダパス（存在しなければ作成）
            string folderPath = GetSelectedPathOrFallback();
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
            }

            // 各ScriptableObjectのインスタンスを作成してアセットとして保存する
            IdleState objA = ScriptableObject.CreateInstance<IdleState>();
            UnityEditor.AssetDatabase.CreateAsset(objA, folderPath + "/Idle.asset");

            WanderState objB = ScriptableObject.CreateInstance<WanderState>();
            UnityEditor.AssetDatabase.CreateAsset(objB, folderPath + "/Wander.asset");

            ApproachState objC = ScriptableObject.CreateInstance<ApproachState>();
            UnityEditor.AssetDatabase.CreateAsset(objC, folderPath + "/Approach.asset");

            AttackState objD = ScriptableObject.CreateInstance<AttackState>();
            UnityEditor.AssetDatabase.CreateAsset(objD, folderPath + "/Attack.asset");

            BlockState objE = ScriptableObject.CreateInstance<BlockState>();
            UnityEditor.AssetDatabase.CreateAsset(objE, folderPath + "/Block.asset");

            DamageState objF = ScriptableObject.CreateInstance<DamageState>();
            UnityEditor.AssetDatabase.CreateAsset(objF, folderPath + "/Damage.asset");

            DeadState objG = ScriptableObject.CreateInstance<DeadState>();
            UnityEditor.AssetDatabase.CreateAsset(objG, folderPath + "/Dead.asset");

            // アセットの保存とリフレッシュ
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        private static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in UnityEditor.Selection.GetFiltered(typeof(UnityEngine.Object), UnityEditor.SelectionMode.Assets))
            {
                path = UnityEditor.AssetDatabase.GetAssetPath(obj);
                if (System.IO.Directory.Exists(path))
                {
                    return path;
                }
            }
            return path;
        }
    }
#endif
}