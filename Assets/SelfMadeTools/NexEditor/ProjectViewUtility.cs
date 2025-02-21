#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace NexEditor
{
    public static class ProjectViewUtility
    {
        public static string GetCurrentDirectory()
        {
            // Public/NonPublic, Static/Instance の両方のメソッドを対象とする
            var flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            // UnityEditor.dll からアセンブリを読み込み
            var asm = Assembly.Load("UnityEditor.dll");
            // 内部クラス UnityEditor.ProjectBrowser を取得
            var typeProjectBrowser = asm.GetType("UnityEditor.ProjectBrowser");
            // Projectビューのウィンドウを取得
            var projectBrowserWindow = EditorWindow.GetWindow(typeProjectBrowser);
            // 非公開メソッド "GetActiveFolderPath" を取得
            var method = typeProjectBrowser.GetMethod("GetActiveFolderPath", flag);
            // メソッドを実行して、現在のディレクトリパスを取得
            return (string)method.Invoke(projectBrowserWindow, null);
        }
    }
}
#endif