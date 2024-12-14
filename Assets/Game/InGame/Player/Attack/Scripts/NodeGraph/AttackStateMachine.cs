using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class AttackStateMachine : IState
    {
        #region IState
        private ComboTree _tree;
        private ComboNode _currentNode;
        private ComboNode _begin;

        public string AnimationName => "";
        public ComboNode CurrentNode => _currentNode;

        public void Initialize(ComboTree tree, ComboTree.NodeType beginNode)
        {
            _tree = tree;
            _currentNode = null;
            _begin = _tree.GetRoot(beginNode);
        }

        public void Enter(PlayerController player)
        {
            player.MovementParameters.Velocity = Vector3.zero;
            ChangeAttackState(player, _begin);
        }

        public void Execute(PlayerController player)
        {
            if (_currentNode == null) // 次のノードがない場合は終了する。
            {
                Debug.LogWarning("Current Node is null");
                this.TransitionToDefaultState(player);
                return;
            }
            if (_currentNode.Behaviour == null) // 次のノードに振る舞いが設定されてない場合は終了する。
            {
                Debug.LogWarning("Current Node Behaviour is null");
                this.TransitionToDefaultState(player);
                return;
            }

            // 入力に応じて回避状態に遷移する。
            if (PlayerInputHandler.InGameInput.Dodge.triggered)
            {
                player.StateMachine.ChangeState<GroundDodge>();
                return;
            }

            _currentNode.Behaviour.Execute(player);
        }

        public void Exit(PlayerController player)
        {
            ExitAttackState(player);
        }
        #endregion

        #region AttackStateMachine
        public void ChangeAttackState(PlayerController player, ComboNode node)
        {
            ExitAttackState(player);
            _currentNode = node;
            EnterAttackState(player);
        }

        private void ExitAttackState(PlayerController player)
        {
            if (_currentNode && _currentNode.Behaviour)
            {
                if (!string.IsNullOrEmpty(_currentNode.Behaviour.AnimationName))
                    player.Animator.SetBool(_currentNode.Behaviour.AnimationName, false);
                _currentNode.Behaviour.Exit(player);
                _currentNode.Behaviour.OnTransitionX -= ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY -= ChangeToYChild;
                _currentNode.Behaviour.OnCompleted -= this.TransitionToDefaultState;
            }
        }

        private void EnterAttackState(PlayerController player)
        {
            if (_currentNode && _currentNode.Behaviour)
            {
                if (!string.IsNullOrEmpty(_currentNode.Behaviour.AnimationName))
                    player.Animator.SetBool(_currentNode.Behaviour.AnimationName, true);
                _currentNode.Behaviour.Enter(player);
                _currentNode.Behaviour.OnTransitionX += ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY += ChangeToYChild;
                _currentNode.Behaviour.OnCompleted += this.TransitionToDefaultState;
            }
        }

        private void ChangeToXChild(PlayerController player)
        {
            ChangeAttackState(player, _currentNode.XChild);
        }

        private void ChangeToYChild(PlayerController player)
        {
            ChangeAttackState(player, _currentNode.YChild);
        }
        #endregion
    }
}