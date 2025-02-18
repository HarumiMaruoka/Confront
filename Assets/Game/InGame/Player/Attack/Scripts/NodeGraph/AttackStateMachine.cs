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

        // プレイヤーステートを変更する前に呼び出す。
        public void Initialize(ComboTree tree, ComboTree.NodeType beginNode)
        {
            _tree = tree;
            _currentNode = null;

            if (tree == null)
            {
                Debug.LogWarning("ComboTree is null");
                return;
            }

            _begin = _tree.GetRoot(beginNode);
        }

        public void Enter(PlayerController player)
        {
            if (_tree == null)
            {
                return;
            }

            player.MovementParameters.Velocity = Vector3.zero;
            ChangeAttackState(player, _begin);
        }

        public void Execute(PlayerController player)
        {
            if (_currentNode == null) // 次のノードがない場合は終了する。
            {
                Debug.LogWarning("Current Node is null");
                AttackCompleted(player);
                return;
            }
            if (_currentNode.Behaviour == null) // 次のノードに振る舞いが設定されてない場合は終了する。
            {
                Debug.LogWarning("Current Node Behaviour is null");
                AttackCompleted(player);
                return;
            }

            // 入力に応じて回避状態に遷移する。
            if (PlayerInputHandler.InGameInput.Dodge.triggered)
            {
                player.StateMachine.ChangeState<GroundDodge>();
                return;
            }

            _currentNode.Behaviour.Execute(player, _tree);
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
                _currentNode.Behaviour.Exit(player, _tree);
                _currentNode.Behaviour.OnTransitionX -= ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY -= ChangeToYChild;
                _currentNode.Behaviour.OnCompleted -= AttackCompleted;
            }
        }

        private void EnterAttackState(PlayerController player)
        {
            if (_currentNode && _currentNode.Behaviour)
            {
                if (!string.IsNullOrEmpty(_currentNode.Behaviour.AnimationName))
                {
                    var animationName = _currentNode.Behaviour.AnimationName;
                    var animationOffset = _currentNode.Behaviour.AnimationOffset;
                    player.Animator.CrossFade(animationName, 0.1f, 0, animationOffset);
                }
                _currentNode.Behaviour.Enter(player, _tree);
                _currentNode.Behaviour.OnTransitionX += ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY += ChangeToYChild;
                _currentNode.Behaviour.OnCompleted += AttackCompleted;
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

        private void AttackCompleted(PlayerController player)
        {
            this.TransitionToDefaultState(player);
            player.MovementParameters.ResetAttackIntervalTimer();
        }
        #endregion
    }
}