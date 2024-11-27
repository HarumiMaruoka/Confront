using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Confront.Player.ComboEditor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public ComboNode node;
        public Port inputPort;
        public Port outputPortX;
        public Port outputPortY;
        public Action<NodeView> OnNodeSelected;

        public NodeView(ComboNode node)
        {
            this.node = node;
            this.viewDataKey = node.Guid;

            bool isRoot = node.NodeType != ComboTree.NodeType.Child;

            style.left = node.Position.x;
            style.top = node.Position.y;

            var middle = new VisualElement();
            middle.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.7f);
            this.Q("node-border").Insert(1, middle);
            middle.Insert(0, new IMGUIContainer(() =>
            {
                var serializedObject = new UnityEditor.SerializedObject(node);
                var serializedProperty = serializedObject.FindProperty("Behaviour");
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent());
                if (serializedObject.ApplyModifiedProperties()) UpdateNodeTitle();
            }));

            SetNodeTypeStyle(node.NodeType);
            UpdateNodeTitle();

            if (!isRoot) CreateInputPorts();
            CreateOutputPorts();
        }

        public void UpdateNodeTitle()
        {
            var behaviorName = node.Behaviour == null ? "null" : node.Behaviour.name;

            title =
                "Behavior: " + behaviorName + "\n" +
                "Combo: " + GetComboName();
        }

        public string GetComboName()
        {
            HashSet<ComboNode> opened = new HashSet<ComboNode>();

            var child = node;
            var parent = node.Parent;
            var comboName = "";
            opened.Add(child);

            while (parent != null)
            {
                comboName = (parent.XChild == child ? "X" : "Y") + comboName;
                child = parent;
                parent = parent.Parent;
                if (opened.Contains(parent)) return "Loop";
            }
            comboName = child.NodeType.Name() + comboName;

            return comboName;
        }

        private void SetNodeTypeStyle(ComboTree.NodeType nodeType)
        {
            if (nodeType == ComboTree.NodeType.GroundRootX)
            {
                this.Q<VisualElement>("title").style.backgroundColor = new Color(1, 0, 0, 0.6f);
            }
            else if (nodeType == ComboTree.NodeType.GroundRootY)
            {
                this.Q<VisualElement>("title").style.backgroundColor = new Color(0, 1, 0, 0.6f);
                this.Q<Label>("title-label").style.color = new Color(0.1f, 0.1f, 0.1f, 1);
            }
            else if (nodeType == ComboTree.NodeType.AirRootX)
            {
                this.Q<VisualElement>("title").style.backgroundColor = new Color(0, 0, 1, 0.6f);
            }
            else if (nodeType == ComboTree.NodeType.AirRootY)
            {
                this.Q<VisualElement>("title").style.backgroundColor = new Color(1, 0.92f, 0.016f, 0.6f);
                this.Q<Label>("title-label").style.color = new Color(0.1f, 0.1f, 0.1f, 1);
            }
        }

        private void CreateInputPorts()
        {
            inputPort = Port.Create<Edge>(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
            inputPort.portName = "";
            inputContainer.Add(inputPort);
        }

        private void CreateOutputPorts()
        {
            outputPortX = Port.Create<Edge>(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPortX.portName = "X";
            outputContainer.Add(outputPortX);
            outputPortY = Port.Create<Edge>(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPortY.portName = "Y";
            outputContainer.Add(outputPortY);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.Position.x = newPos.xMin;
            node.Position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}