using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class ComboNode : ScriptableObject
    {
        [HideInInspector]
        public ComboNode Parent;
        [HideInInspector]
        public ComboNode XChild;
        [HideInInspector]
        public ComboNode YChild;

        [HideInInspector]
        public string Guid;
        [HideInInspector]
        public Vector2 Position;
        [HideInInspector]
        public ComboTree.NodeType NodeType;

        public AttackBehaviour Behaviour;

        // ツリー構造を複製する。基本的にはRootノードから複製することを想定している。
        public ComboNode DuplicateNode(ComboNode original)
        {
            if (original == null) return null;

            var clone = CreateInstance<ComboNode>();
            clone.Behaviour = Instantiate(original.Behaviour);
            clone.NodeType = original.NodeType;
            clone.Position = original.Position;
            clone.XChild = DuplicateNode(original.XChild);
            clone.YChild = DuplicateNode(original.YChild);
            return clone;
        }
    }
}