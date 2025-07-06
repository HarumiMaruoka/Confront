using System;
using UnityEngine;


namespace NexEditor.ScriptableDashboard
{
    public static class FileTemplate
    {
        public static string Data(string dataName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            // Data file template without namespace
            $"using System;                              \r\n" +
            $"using UnityEngine;                         \r\n" +
            $"                                           \r\n" +
            $"public class {dataName} : ScriptableObject \r\n" +
            $"{{                                         \r\n" +
            $"                                           \r\n" +
            $"}}                                         " :

            // Data file template with namespace
            $"using System;                                  \r\n" +
            $"using UnityEngine;                             \r\n" +
            $"                                               \r\n" +
            $"namespace {@namespace}                         \r\n" +
            $"{{                                             \r\n" +
            $"    public class {dataName} : ScriptableObject \r\n" +
            $"    {{                                         \r\n" +
            $"                                               \r\n" +
            $"    }}                                         \r\n" +
            $"}}                                             ";

        public static string Dashboard(string dashboardName, string dataName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            // Sheet file template without namespace
            $"using System;                                                                                         \r\n" +
            $"using UnityEngine;                                                                                    \r\n" +
            $"using NexEditor.ScriptableDashboard;                                                                  \r\n" +
            $"                                                                                                      \r\n" +
            $"[CreateAssetMenu(fileName = \"{dashboardName}\",menuName = \"Scriptable Dashboard/{dashboardName}\")] \r\n" +
            $"public class {dashboardName} : ScriptableDashboard<{dataName}>                                        \r\n" +
            $"{{                                                                                                    \r\n" +
            $"                                                                                                      \r\n" +
            $"}}                                                                                                        " :

            // Sheet file template with namespace
            $"using System;                                                                                             \r\n" +
            $"using UnityEngine;                                                                                        \r\n" +
            $"using NexEditor.ScriptableDashboard;                                                                      \r\n" +
            $"                                                                                                          \r\n" +
            $"namespace {@namespace}                                                                                    \r\n" +
            $"{{                                                                                                        \r\n" +
            $"    [CreateAssetMenu(fileName = \"{dashboardName}\",menuName = \"Scriptable Dashboard/{dashboardName}\")] \r\n" +
            $"    public class {dashboardName} : ScriptableDashboard<{dataName}>                                        \r\n" +
            $"    {{                                                                                                    \r\n" +
            $"                                                                                                          \r\n" +
            $"    }}                                                                                                    \r\n" +
            $"}}                                                                                                            ";

        public static string Window(string windowName, string dataName, string dashboardName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            $"#if UNITY_EDITOR                                                                   \r\n" +
            $"using System;                                                                      \r\n" +
            $"using UnityEditor;                                                                 \r\n" +
            $"using UnityEngine;                                                                 \r\n" +
            $"using NexEditor.ScriptableDashboard;                                               \r\n" +
            $"                                                                                   \r\n" +
            $"public class {windowName} : ScriptableDashboardEditor<{dataName}>                  \r\n" +
            $"{{                                                                                 \r\n" +
            $"    [MenuItem(\"Window/Scriptable Dashboard/{windowName}\")]                       \r\n" +
            $"    public static void ShowWindow()                                                \r\n" +
            $"    {{                                                                             \r\n" +
            $"        {windowName} window = GetWindow<{windowName}>();                           \r\n" +
            $"        string[] guids = AssetDatabase.FindAssets(\"t:{dashboardName}\");          \r\n" +
            $"        if (guids.Length > 0)                                                      \r\n" +
            $"        {{                                                                         \r\n" +
            $"            string path = AssetDatabase.GUIDToAssetPath(guids[0]);                 \r\n" +
            $"            var dashboard = AssetDatabase.LoadAssetAtPath<{dashboardName}>(path);  \r\n" +
            $"            if (dashboard != null)                                                 \r\n" +
            $"            {{                                                                     \r\n" +
            $"                window.Setup(dashboard);                                           \r\n" +
            $"            }}                                                                     \r\n" +
            $"            else                                                                   \r\n" +
            $"            {{                                                                     \r\n" +
            $"                Debug.LogWarning(\"{dashboardName} not found at path: \" + path); \r\n" +
            $"            }}                                                                     \r\n" +
            $"        }}                                                                         \r\n" +
            $"        else                                                                       \r\n" +
            $"        {{                                                                         \r\n" +
            $"            Debug.LogWarning(\"No {dashboardName} found in the project.\");       \r\n" +
            $"        }}                                                                         \r\n" +
            $"    }}                                                                             \r\n" +
            $"                                                                                   \r\n" +
            $"    public static void ShowWindow({dashboardName} dashboard)                       \r\n" +
            $"    {{                                                                             \r\n" +
            $"        {windowName} window = GetWindow<{windowName}>();                           \r\n" +
            $"        window.Setup(dashboard);                                                   \r\n" +
            $"    }}                                                                             \r\n" +
            $"}}                                                                                 \r\n" +
            $"                                                                                   \r\n" +
            $"[UnityEditor.CustomEditor(typeof({dashboardName}))]                                \r\n" +
            $"public class {dashboardName}Drawer : UnityEditor.Editor                            \r\n" +
            $"{{                                                                                 \r\n" +
            $"    public override void OnInspectorGUI()                                          \r\n" +
            $"    {{                                                                             \r\n" +
            $"        base.OnInspectorGUI();                                                     \r\n" +
            $"                                                                                   \r\n" +
            $"        if (GUILayout.Button(\"Open Window\"))                                     \r\n" +
            $"        {{                                                                         \r\n" +
            $"            var dashboard = target as {dashboardName};                             \r\n" +
            $"            if (dashboard != null)                                                 \r\n" +
            $"            {{                                                                     \r\n" +
            $"                {windowName}.ShowWindow(dashboard);                                \r\n" +
            $"            }}                                                                     \r\n" +
            $"        }}                                                                         \r\n" +
            $"    }}                                                                             \r\n" +
            $"                                                                                   \r\n" +
            $"    [UnityEditor.Callbacks.OnOpenAsset]                                            \r\n" +
            $"    public static bool OnOpenAsset(int instanceId, int line)                       \r\n" +
            $"    {{                                                                             \r\n" +
            $"        if (UnityEditor.Selection.activeObject is {dashboardName} dashboard)       \r\n" +
            $"        {{                                                                         \r\n" +
            $"            {windowName}.ShowWindow(dashboard);                                    \r\n" +
            $"            return true;                                                           \r\n" +
            $"        }}                                                                         \r\n" +
            $"        return false;                                                              \r\n" +
            $"    }}                                                                             \r\n" +
            $"}}                                                                                 \r\n" +
            $"#endif                                                                                 " :

            $"#if UNITY_EDITOR                                                                       \r\n" +
            $"using System;                                                                          \r\n" +
            $"using UnityEditor;                                                                     \r\n" +
            $"using UnityEngine;                                                                     \r\n" +
            $"using NexEditor.ScriptableDashboard;                                                   \r\n" +
            $"                                                                                       \r\n" +
            $"namespace {@namespace}                                                                 \r\n" +
            $"{{                                                                                     \r\n" +
            $"    public class {windowName} : ScriptableDashboardEditor<{dataName}>                  \r\n" +
            $"    {{                                                                                 \r\n" +
            $"        [MenuItem(\"Window/Scriptable Dashboard/{windowName}\")]                       \r\n" +
            $"        public static void ShowWindow()                                                \r\n" +
            $"        {{                                                                             \r\n" +
            $"            {windowName} window = GetWindow<{windowName}>();                           \r\n" +
            $"            string[] guids = AssetDatabase.FindAssets(\"t:{dashboardName}\");          \r\n" +
            $"            if (guids.Length > 0)                                                      \r\n" +
            $"            {{                                                                         \r\n" +
            $"                string path = AssetDatabase.GUIDToAssetPath(guids[0]);                 \r\n" +
            $"                var dashboard = AssetDatabase.LoadAssetAtPath<{dashboardName}>(path);  \r\n" +
            $"                if (dashboard != null)                                                 \r\n" +
            $"                {{                                                                     \r\n" +
            $"                    window.Setup(dashboard);                                           \r\n" +
            $"                }}                                                                     \r\n" +
            $"                else                                                                   \r\n" +
            $"                {{                                                                     \r\n" +
            $"                    Debug.LogWarning(\"{dashboardName} not found at path: \" + path); \r\n" +
            $"                }}                                                                     \r\n" +
            $"            }}                                                                         \r\n" +
            $"            else                                                                       \r\n" +
            $"            {{                                                                         \r\n" +
            $"                Debug.LogWarning(\"No {dashboardName} found in the project.\");       \r\n" +
            $"            }}                                                                         \r\n" +
            $"        }}                                                                             \r\n" +
            $"        public static void ShowWindow({dashboardName} dashboard)                       \r\n" +
            $"        {{                                                                             \r\n" +
            $"            {windowName} window = GetWindow<{windowName}>();                           \r\n" +
            $"            window.Setup(dashboard);                                                   \r\n" +
            $"        }}                                                                             \r\n" +
            $"    }}                                                                                 \r\n" +
            $"                                                                                       \r\n" +
            $"    [UnityEditor.CustomEditor(typeof({dashboardName}))]                                \r\n" +
            $"    public class {dashboardName}Drawer : UnityEditor.Editor                            \r\n" +
            $"    {{                                                                                 \r\n" +
            $"        public override void OnInspectorGUI()                                          \r\n" +
            $"        {{                                                                             \r\n" +
            $"            base.OnInspectorGUI();                                                     \r\n" +
            $"                                                                                       \r\n" +
            $"            if (GUILayout.Button(\"Open Window\"))                                     \r\n" +
            $"            {{                                                                         \r\n" +
            $"                var dashboard = target as {dashboardName};                             \r\n" +
            $"                if (dashboard != null)                                                 \r\n" +
            $"                {{                                                                     \r\n" +
            $"                    {windowName}.ShowWindow(dashboard);                                \r\n" +
            $"                }}                                                                     \r\n" +
            $"            }}                                                                         \r\n" +
            $"        }}                                                                             \r\n" +
            $"                                                                                       \r\n" +
            $"        [UnityEditor.Callbacks.OnOpenAsset]                                            \r\n" +
            $"        public static bool OnOpenAsset(int instanceId, int line)                       \r\n" +
            $"        {{                                                                             \r\n" +
            $"            if (UnityEditor.Selection.activeObject is {dashboardName} dashboard)       \r\n" +
            $"            {{                                                                         \r\n" +
            $"                {windowName}.ShowWindow(dashboard);                                    \r\n" +
            $"                return true;                                                           \r\n" +
            $"            }}                                                                         \r\n" +
            $"            return false;                                                              \r\n" +
            $"        }}                                                                             \r\n" +
            $"    }}                                                                                 \r\n" +
            $"}}                                                                                     \r\n" +
            $"#endif                                                                                     ";
    }
}