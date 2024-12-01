using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class AttackStateMachine : IState
    {
        private ComboTree _tree;
        private ComboNode _currentNode;
        private ComboNode _begin;

        public string AnimationName => "";

        public void Initialize(ComboTree tree, ComboTree.NodeType beginNode)
        {
            _tree = tree;
            _currentNode = null;
            _begin = _tree.GetRoot(beginNode);
        }

        public void Enter(PlayerController player)
        {
            player.MovementParameters.Velocity = Vector3.zero;
            ChangeState(player, _begin);
        }

        public void Execute(PlayerController player)
        {
            if (_currentNode == null)
            {
                Debug.LogError("Current Node is null");
                ChangePlayerState(player);
                return;
            }
            _currentNode.Behaviour.Execute(player);
        }

        public void Exit(PlayerController player)
        {
            ExitCurrentNode(player);
        }

        public void ChangeState(PlayerController player, ComboNode node)
        {
            ExitCurrentNode(player);
            _currentNode = node;
            EnterCurrentNode(player);
        }

        private void ExitCurrentNode(PlayerController player)
        {
            if (_currentNode)
            {
                player.Animator.SetBool(_currentNode.Behaviour.AnimationName, false);
                _currentNode.Behaviour.Exit(player);
                _currentNode.Behaviour.OnTransitionX -= ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY -= ChangeToYChild;
                _currentNode.Behaviour.OnCompleted -= ChangePlayerState;
            }
        }

        private void EnterCurrentNode(PlayerController player)
        {
            if (_currentNode)
            {
                player.Animator.SetBool(_currentNode.Behaviour.AnimationName, true);
                _currentNode.Behaviour.Enter(player);
                _currentNode.Behaviour.OnTransitionX += ChangeToXChild;
                _currentNode.Behaviour.OnTransitionY += ChangeToYChild;
                _currentNode.Behaviour.OnCompleted += ChangePlayerState;
            }
        }

        private void ChangeToXChild(PlayerController player)
        {
            ChangeState(player, _currentNode.XChild);
        }

        private void ChangeToYChild(PlayerController player)
        {
            ChangeState(player, _currentNode.YChild);
        }

        private void ChangePlayerState(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
                case GroundType.Ground: player.StateMachine.ChangeState<Grounded>(); break;
                case GroundType.Abyss: player.StateMachine.ChangeState<Abyss>(); break;
                case GroundType.SteepSlope: player.StateMachine.ChangeState<SteepSlope>(); break;
                case GroundType.InAir: player.StateMachine.ChangeState<InAir>(); break;
            }
        }
    }
}