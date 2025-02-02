using UnityEngine;
using UnityEditor;

public class RemoveCollidersFromPrefab
{
    // Prefab 編集モード中にのみ有効になるメニュー項目
    [MenuItem("Tools/Self Made Tools/Remove All Colliders in Prefab", true)]
    private static bool RemoveCollidersValidate()
    {
        return UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null;
    }

    // メニュー項目「Tools/Remove All Colliders in Prefab」
    [MenuItem("Tools/Self Made Tools/Remove All Colliders in Prefab")]
    private static void RemoveAllColliders()
    {
        // 現在の Prefab 編集モードのステージを取得
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            Debug.LogWarning("Prefab 編集モードではありません。Prefab を開いた状態で実行してください。");
            return;
        }

        // プレハブのルートオブジェクトを取得
        GameObject root = prefabStage.prefabContentsRoot;
        if (root == null)
        {
            Debug.LogWarning("Prefab ルートが見つかりませんでした。");
            return;
        }

        // Undo 登録（階層全体）
        Undo.RegisterFullObjectHierarchyUndo(root, "Remove Colliders");

        // 3D 用 Collider を全て取得して削除
        Collider[] colliders = root.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders)
        {
            // Undo 対応付きで即時削除
            Undo.DestroyObjectImmediate(col);
        }

        Debug.Log("Prefab 内のすべてのコライダーを削除しました。");
    }
}
