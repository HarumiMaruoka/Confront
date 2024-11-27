using Confront.Player.Combo;
using System;
using UnityEngine;

namespace Confront.Player
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

        // �c���[�\���𕡐�����B��{�I�ɂ�Root�m�[�h���畡�����邱�Ƃ�z�肵�Ă���B
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