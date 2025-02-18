using Confront.AttackUtility;
using Confront.VFXSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "ComboTree", menuName = "ConfrontSO/Player/Combo/ComboTree", order = -100)]
    public class ComboTree : ScriptableObject
    {
        public ComboNode GroundRootX;
        public ComboNode GroundRootY;
        public ComboNode AirRootX;
        public ComboNode AirRootY;

        public VFXParameters DefaultVFX;
        public AudioClip[] DefaultHitSFXs;
        public AudioClip DefaultHitSFX
        {
            get
            {
                if (DefaultHitSFXs == null || DefaultHitSFXs.Length == 0) return null;
                return DefaultHitSFXs[UnityEngine.Random.Range(0, DefaultHitSFXs.Length)];
            }
        }

        public event Action<ComboNode> OnRootDuplicated;

        // NodeGraphGUIで使用する変数
        [HideInInspector]
        public Vector3 Position = Vector3.zero;
        [HideInInspector]
        public Vector3 Scale = Vector3.one;

        public List<ComboNode> Children = new List<ComboNode>();

        public ComboTree DuplicateTree()
        {
            var clone = CreateInstance<ComboTree>();
            clone.GroundRootX = GroundRootX?.DuplicateNode(GroundRootX);
            clone.GroundRootY = GroundRootY?.DuplicateNode(GroundRootY);
            clone.AirRootX = AirRootX?.DuplicateNode(AirRootX);
            clone.AirRootY = AirRootY?.DuplicateNode(AirRootY);
            return clone;
        }
        public enum InputType { X, Y }
        [Serializable]
        public enum NodeType { GroundRootX, GroundRootY, AirRootX, AirRootY, Child }

#if UNITY_EDITOR
        public ComboNode CreateNode(NodeType createStateType, Vector2 position = default)
        {
            var node = ScriptableObject.CreateInstance<ComboNode>();
            if (node == null) throw new Exception("Type is not AttackState");

            node.NodeType = createStateType;
            node.Position = createStateType switch
            {
                NodeType.GroundRootX => new Vector2(100, 100),
                NodeType.GroundRootY => new Vector2(100, 250),
                NodeType.AirRootX => new Vector2(100, 400),
                NodeType.AirRootY => new Vector2(100, 550),
                _ => position,
            };

            AddAttackState(node);

            UnityEditor.AssetDatabase.AddObjectToAsset(node, this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            node.Guid = UnityEditor.GUID.Generate().ToString();

            return node;
        }

        public void DeleteAttackState(ComboNode attackState)
        {
            if (attackState == GroundRootX)
            {
                GroundRootX = null;
            }
            else if (attackState == GroundRootY)
            {
                GroundRootY = null;
            }
            else if (attackState == AirRootX)
            {
                AirRootX = null;
            }
            else if (attackState == AirRootY)
            {
                AirRootY = null;
            }
            else
            {
                Children.Remove(attackState);
            }

            UnityEditor.AssetDatabase.RemoveObjectFromAsset(attackState);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        public void Connect(ComboNode parent, InputType input, ComboNode child)
        {
            // ChildがRootの場合は接続しない
            if (child == GroundRootX || child == GroundRootY || child == AirRootX || child == AirRootY)
            {
                Debug.LogWarning("Root cannot be connected as a child");
                return;
            }

            if (input == InputType.X)
            {
                parent.XChild = child;
                child.Parent = parent;
            }
            else
            {
                parent.YChild = child;
                child.Parent = parent;
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void Disconnect(ComboNode parent, InputType input, ComboNode child)
        {
            if (input == InputType.X)
            {
                parent.XChild = null;
                child.Parent = null;
            }
            else
            {
                parent.YChild = null;
                child.Parent = null;
            }

            UnityEditor.EditorUtility.SetDirty(this);

        }

        public ComboNode GetParent(ComboNode child)
        {
            return child.Parent;
        }

        public ComboNode GetChild(ComboNode parent, InputType input)
        {
            return input == InputType.X ? parent.XChild : parent.YChild;
        }

        private void AddAttackState(ComboNode attackState)
        {
            switch (attackState.NodeType)
            {
                case NodeType.GroundRootX:
                    if (GroundRootX != null)
                    {
                        OnRootDuplicated?.Invoke(GroundRootX);
                        Debug.LogWarning("RootX already exists");
                    }
                    GroundRootX = attackState;
                    break;
                case NodeType.GroundRootY:
                    if (GroundRootY != null)
                    {
                        OnRootDuplicated?.Invoke(GroundRootY);
                        Debug.LogWarning("RootY already exists");
                    }
                    GroundRootY = attackState;
                    break;
                case NodeType.AirRootX:
                    if (AirRootX != null)
                    {
                        OnRootDuplicated?.Invoke(AirRootX);
                        Debug.LogWarning("RootX already exists");
                    }
                    AirRootX = attackState;
                    break;
                case NodeType.AirRootY:
                    if (AirRootY != null)
                    {
                        OnRootDuplicated?.Invoke(AirRootY);
                        Debug.LogWarning("RootY already exists");
                    }
                    AirRootY = attackState;
                    break;
                case NodeType.Child:
                    Children.Add(attackState);
                    break;
            }
        }
#endif

        public ComboNode GetRoot(NodeType nodeType)
        {
            return nodeType switch
            {
                NodeType.GroundRootX => GroundRootX,
                NodeType.GroundRootY => GroundRootY,
                NodeType.AirRootX => AirRootX,
                NodeType.AirRootY => AirRootY,
                _ => null,
            };
        }
    }
    public static class NodeTypeExtension
    {
        public static string Name(this ComboTree.NodeType nodeType)
        {
            switch (nodeType)
            {
                case ComboTree.NodeType.GroundRootX: return "GroundX";
                case ComboTree.NodeType.GroundRootY: return "GroundY";
                case ComboTree.NodeType.AirRootX: return "AirX";
                case ComboTree.NodeType.AirRootY: return "AirY";
                case ComboTree.NodeType.Child: return "Child";
                default: return "Unknown";
            }
        }
    }
}