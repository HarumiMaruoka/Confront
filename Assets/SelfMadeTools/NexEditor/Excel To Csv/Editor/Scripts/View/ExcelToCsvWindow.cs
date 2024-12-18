using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NexEditor.ExcelToCsv
{
    public class ExcelToCsvWindow : EditorWindow
    {
        [SerializeField]
        private DefaultAsset _excelToCsvAll;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private Converter _converter;

        [MenuItem("Window/Excel To Csv")]
        public static void ShowExample()
        {
            ExcelToCsvWindow wnd = GetWindow<ExcelToCsvWindow>();
            wnd.titleContent = new GUIContent("Excel To Csv");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);

            // Initialize
            _converter = new Converter(AssetDatabase.GetAssetPath(_excelToCsvAll));
            UIManager uiManager = new UIManager();
            uiManager.Bind(labelFromUXML, _converter);
        }
    }
}