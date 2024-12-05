using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MissingComponentRemover
{
    [MenuItem("Tools/Self Made Tools/Remove Missing Components in Scene")]
    private static void RemoveMissingComponentsInScene()
    {
        // プレイモードかどうかをチェック
        if (Application.isPlaying)
        {
            Debug.LogWarning("プレイモード中はこの操作を実行できません。");
            return;
        }

        // プレハブ編集モードかどうかをチェック
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            Debug.LogWarning("プレハブ編集モードではこの操作を実行できません。");
            return;
        }

        // シーン内のすべてのルートオブジェクトを取得
        var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        // 各ルートオブジェクトについてミッシングコンポーネントを削除
        foreach (var rootGameObject in rootGameObjects)
        {
            RemoveMissingComponents(rootGameObject);
        }

        Debug.Log("シーン内のミッシングなコンポーネントを削除しました。");
    }

    [MenuItem("Tools/Self Made Tools/Remove Missing Components in Prefab")]
    private static void RemoveMissingComponentsInPrefab()
    {
        // 現在のプレハブステージを取得
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            Debug.LogWarning("Prefab編集モードではありません。");
            return;
        }

        // ルートゲームオブジェクトを取得
        var rootGameObject = prefabStage.prefabContentsRoot;
        RemoveMissingComponents(rootGameObject);

        // 変更を保存
        PrefabUtility.SaveAsPrefabAsset(rootGameObject, prefabStage.assetPath);
        Debug.Log("Missingなコンポーネントを削除しました。");
    }

    private static void RemoveMissingComponents(GameObject gameObject)
    {
        // Missingなコンポーネントを削除
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);

        // 子オブジェクトにも適用
        foreach (Transform child in gameObject.transform)
        {
            RemoveMissingComponents(child.gameObject);
        }
    }
}
