using System;
using UnityEngine;


namespace NexEditor.GameDataSheet
{
    public static class FileTemplate
    {
        public static string Data(string dataName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            $"using System;                              \r\n" +
            $"using UnityEngine;                         \r\n" +
            $"                                           \r\n" +
            $"public class {dataName} : ScriptableObject \r\n" +
            $"{{                                         \r\n" +
            $"                                           \r\n" +
            $"}}                                         " :

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

        public static string Sheet(string sheetName, string dataName, string windowName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            $"using System;                                                                             \r\n" +
            $"using UnityEngine;                                                                        \r\n" +
            $"                                                                                          \r\n" +
            $"[CreateAssetMenu(fileName = \"{sheetName}\",menuName = \"Game Data Sheets/{sheetName}\")] \r\n" +
            $"public class {sheetName} : NexEditor.GameDataSheet.SheetBase<{dataName}>                       \r\n" +
            $"{{                                                                                        \r\n" +
            $"                                                                                          \r\n" +
            $"}}                                                                                        \r\n" +
            $"                                                                                          \r\n" +
            $"                                                                                          \r\n" +
            $"#if UNITY_EDITOR                                                                          \r\n" +
            $"[UnityEditor.CustomEditor(typeof({sheetName}))]                                           \r\n" +
            $"public class {sheetName}Drawer : UnityEditor.Editor                                       \r\n" +
            $"{{                                                                                        \r\n" +
            $"    public override void OnInspectorGUI()                                                 \r\n" +
            $"    {{                                                                                    \r\n" +
            $"        if (GUILayout.Button(\"Open Window\"))                                            \r\n" +
            $"        {{                                                                                \r\n" +
            $"            {windowName}.Init();                                                          \r\n" +
            $"        }}                                                                                \r\n" +
            $"                                                                                          \r\n" +
            $"        base.OnInspectorGUI();                                                            \r\n" +
            $"    }}                                                                                    \r\n" +
            $"}}                                                                                        \r\n" +
            $"#endif                                                                                    " :

            $"using System;                                                                                 \r\n" +
            $"using UnityEngine;                                                                            \r\n" +
            $"                                                                                              \r\n" +
            $"namespace {@namespace}                                                                        \r\n" +
            $"{{                                                                                            \r\n" +
            $"    [CreateAssetMenu(fileName = \"{sheetName}\",menuName = \"Game Data Sheets/{sheetName}\")] \r\n" +
            $"    public class {sheetName} : NexEditor.GameDataSheet.SheetBase<{dataName}>                       \r\n" +
            $"    {{                                                                                        \r\n" +
            $"                                                                                              \r\n" +
            $"    }}                                                                                        \r\n" +
            $"                                                                                              \r\n" +
            $"    #if UNITY_EDITOR                                                                          \r\n" +
            $"    [UnityEditor.CustomEditor(typeof({sheetName}))]                                           \r\n" +
            $"    public class {sheetName}Drawer : UnityEditor.Editor                                       \r\n" +
            $"    {{                                                                                        \r\n" +
            $"        public override void OnInspectorGUI()                                                 \r\n" +
            $"        {{                                                                                    \r\n" +
            $"            if (GUILayout.Button(\"Open Window\"))                                            \r\n" +
            $"            {{                                                                                \r\n" +
            $"                {windowName}.Init();                                                          \r\n" +
            $"            }}                                                                                \r\n" +
            $"                                                                                              \r\n" +
            $"            base.OnInspectorGUI();                                                            \r\n" +
            $"        }}                                                                                    \r\n" +
            $"    }}                                                                                        \r\n" +
            $"    #endif                                                                                    \r\n" +
            $"}}                                                                                            ";

        public static string WindowLayout(string layoutName, string dataName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            $"#if UNITY_EDITOR                                                              \r\n" +
            $"using System;                                                                 \r\n" +
            $"using UnityEngine;                                                            \r\n" +
            $"                                                                              \r\n" +
            $"public class {layoutName} : NexEditor.GameDataSheet.WindowLayout<{dataName}> {{ }} \r\n" +
            $"#endif                                                                            " :

            $"#if UNITY_EDITOR                                                                  \r\n" +
            $"using System;                                                                     \r\n" +
            $"using UnityEngine;                                                                \r\n" +
            $"                                                                                  \r\n" +
            $"namespace {@namespace}                                                            \r\n" +
            $"{{                                                                                \r\n" +
            $"    public class {layoutName} : NexEditor.GameDataSheet.WindowLayout<{dataName}> {{ }} \r\n" +
            $"}}                                                                                \r\n" +
            $"#endif                                                                            ";

        public static string Window(string windowName, string dataName, string sheetName, string layoutName, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
            $"#if UNITY_EDITOR                                                                                      \r\n" +
            $"using System;                                                                                         \r\n" +
            $"using UnityEditor;                                                                                    \r\n" +
            $"using UnityEngine;                                                                                    \r\n" +
            $"                                                                                                      \r\n" +
            $"public class {windowName} : NexEditor.GameDataSheet.SheetWindowBase<{dataName}, {sheetName}, {layoutName}> \r\n" +
            $"{{                                                                                                    \r\n" +
            $"    [MenuItem(\"Window/Game Data Sheet/{windowName}\")]                                               \r\n" +
            $"    public static void Init()                                                                         \r\n" +
            $"    {{                                                                                                \r\n" +
            $"        GetWindow(typeof({windowName})).Show();                                                       \r\n" +
            $"    }}                                                                                                \r\n" +
            $"}}                                                                                                    \r\n" +
            $"#endif                                                                                                    " :

            $"#if UNITY_EDITOR                                                                                          \r\n" +
            $"using System;                                                                                             \r\n" +
            $"using UnityEditor;                                                                                        \r\n" +
            $"using UnityEngine;                                                                                        \r\n" +
            $"                                                                                                          \r\n" +
            $"namespace {@namespace}                                                                                    \r\n" +
            $"{{                                                                                                        \r\n" +
            $"    public class {windowName} : NexEditor.GameDataSheet.SheetWindowBase<{dataName}, {sheetName}, {layoutName}> \r\n" +
            $"    {{                                                                                                    \r\n" +
            $"        [MenuItem(\"Window/Game Data Sheet/{windowName}\")]                                               \r\n" +
            $"        public static void Init()                                                                         \r\n" +
            $"        {{                                                                                                \r\n" +
            $"            GetWindow(typeof({windowName})).Show();                                                       \r\n" +
            $"        }}                                                                                                \r\n" +
            $"    }}                                                                                                    \r\n" +
            $"}}                                                                                                        \r\n" +
            $"#endif                                                                                                    ";
    }
}