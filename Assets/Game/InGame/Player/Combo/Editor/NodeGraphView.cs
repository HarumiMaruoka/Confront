using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Confront.Player.Combo;

namespace Confront.Player.ComboEditor
{
    public class NodeGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeGraphView, GraphView.UxmlTraits> { }

        private ComboTree _comboTree;

        public Action<NodeView> OnNodeSelected;

        public NodeGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public void Initialize(StyleSheet styleSheet)
        {
            styleSheets.Add(styleSheet);
        }

        private NodeView FindNodeView(ComboNode node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }

        public void PopulateView(ComboTree tree)
        {
            if (_comboTree)
            {
                _comboTree.Position = viewTransform.position;
                _comboTree.Scale = viewTransform.scale;
                _comboTree.OnRootDuplicated -= OnRootNodeDuplicated;
            }
            _comboTree = tree;
            if (_comboTree)
            {
                var position = _comboTree.Position;
                var scale = _comboTree.Scale;
                viewTransform.position = position;
                viewTransform.scale = scale;
                _comboTree.OnRootDuplicated += OnRootNodeDuplicated;
            }

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            // Creates node view
            InitTree(tree);
            CreateNodeView(tree.GroundRootX);
            CreateNodeView(tree.GroundRootY);
            CreateNodeView(tree.AirRootX);
            CreateNodeView(tree.AirRootY);
            foreach (var child in tree.Children) CreateNodeView(child);

            // Creates edge
            foreach (var child in tree.Children)
            {
                var parent = child.Parent;
                if (parent == null) continue;

                var parentView = FindNodeView(parent);
                var input = parent.XChild == child ? ComboTree.InputType.X : ComboTree.InputType.Y;
                var childView = FindNodeView(child);

                Edge edge;
                if (input == ComboTree.InputType.X) edge = parentView.outputPortX.ConnectTo(childView.inputPort);
                else edge = parentView.outputPortY.ConnectTo(childView.inputPort);
                AddElement(edge);
            }
        }

        private void InitTree(ComboTree tree)
        {
            tree.GroundRootX = tree.GroundRootX != null ? tree.GroundRootX : tree.CreateNode(ComboTree.NodeType.GroundRootX);
            tree.GroundRootY = tree.GroundRootY != null ? tree.GroundRootY : tree.CreateNode(ComboTree.NodeType.GroundRootY);
            tree.AirRootX = tree.AirRootX != null ? tree.AirRootX : tree.CreateNode(ComboTree.NodeType.AirRootX);
            tree.AirRootY = tree.AirRootY != null ? tree.AirRootY : tree.CreateNode(ComboTree.NodeType.AirRootY);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    var nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        _comboTree.DeleteAttackState(nodeView.node);
                    }

                    var edge = elem as Edge;
                    if (edge != null)
                    {
                        var outputNodeView = edge.output.node as NodeView;
                        var inputNodeView = edge.input.node as NodeView;
                        if (outputNodeView != null && inputNodeView != null)
                        {
                            var input = edge.output.portName == "X" ? ComboTree.InputType.X : ComboTree.InputType.Y;
                            _comboTree.Disconnect(outputNodeView.node, input, inputNodeView.node);
                        }
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    var outputNodeView = edge.output.node as NodeView;
                    var inputNodeView = edge.input.node as NodeView;
                    if (outputNodeView != null && inputNodeView != null)
                    {
                        var input = edge.output.portName == "X" ? ComboTree.InputType.X : ComboTree.InputType.Y;
                        _comboTree.Connect(outputNodeView.node, input, inputNodeView.node);
                        inputNodeView.UpdateNodeTitle();
                        outputNodeView.UpdateNodeTitle();
                    }
                });
            }

            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (!_comboTree)
            {
                UnityEngine.Debug.Log("ComboTree is not selected.");
                return;
            }

            var nodePosition = this.contentViewContainer.WorldToLocal(evt.mousePosition);
            evt.menu.AppendAction($"Create", (e) => CreateNode(ComboTree.NodeType.Child, nodePosition));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }

        private void CreateNode(ComboTree.NodeType nodeType, Vector2 position)
        {
            var node = _comboTree.CreateNode(nodeType, position);
            CreateNodeView(node);
        }

        private void CreateNodeView(ComboNode attackState)
        {
            if (attackState == null) return;

            var nodeView = new NodeView(attackState);
            nodeView.OnNodeSelected = OnNodeSelected;

            AddElement(nodeView);
        }

        private void OnRootNodeDuplicated(ComboNode duplicatedNode)
        {
            var nodeView = FindNodeView(duplicatedNode);
            if (nodeView != null)
            {
                var edge = nodeView.outputPortX.connections.FirstOrDefault();
                if (edge != null)
                {
                    var childView = edge.input.node as NodeView;
                    childView.node.Parent = null;

                    edge.input.Disconnect(edge);
                    RemoveElement(edge);
                }
                edge = nodeView.outputPortY.connections.FirstOrDefault();
                if (edge != null)
                {
                    var childView = edge.input.node as NodeView;
                    childView.node.Parent = null;
                    edge.input.Disconnect(edge);
                    RemoveElement(edge);
                }

                _comboTree.DeleteAttackState(duplicatedNode);
                RemoveElement(nodeView);
            }
        }

        public void Close()
        {
            if (_comboTree)
            {
                _comboTree.Position = viewTransform.position;
                _comboTree.Scale = viewTransform.scale;
                _comboTree.OnRootDuplicated -= OnRootNodeDuplicated;
            }
        }
    }
}