using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using Confront.Player.Combo;

namespace Confront.Player.ComboEditor
{
    public class ComboEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset _windowUxml = default;
        [SerializeField]
        private StyleSheet _windowUss = default;
        [SerializeField]
        private StyleSheet _nodeGraphUss = default;

        private NodeGraphView _nodeGraphView;
        private InspectorView _inspectorView;

        [MenuItem("Window/UI Toolkit/ComboEditorWindow")]
        public static void ShowWindow()
        {
            ComboEditorWindow wnd = GetWindow<ComboEditorWindow>();
            wnd.titleContent = new GUIContent("ComboEditorWindow");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is ComboTree)
            {
                ShowWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            _windowUxml.CloneTree(root);
            root.styleSheets.Add(_windowUss);

            _nodeGraphView = root.Q<NodeGraphView>();
            _nodeGraphView.Initialize(_nodeGraphUss);
            _nodeGraphView.OnNodeSelected = OnNodeSelectionChanged;

            _inspectorView = root.Q<InspectorView>();

            OnSelectionChange();
        }

        private void OnDestroy()
        {
            if (_nodeGraphView != null) _nodeGraphView.Close();
        }

        private void OnSelectionChange()
        {
            ComboTree comboTree = Selection.activeObject as ComboTree;
            if (comboTree)
            {
                _nodeGraphView.PopulateView(comboTree);
                return;
            }

            var gameObject = Selection.activeObject as GameObject;
            var player = gameObject?.GetComponent<PlayerController>();
            if (player && player.AttackComboTree)
            {
                _nodeGraphView.PopulateView(player.AttackComboTree);
                return;
            }
        }

        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
}