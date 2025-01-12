using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public abstract class TransitionableStateBase : ScriptableObject
    {
        [Header("Transition")]
        [SerializeField]
        private StateTable _nextStateTable;
        [Range(0f, 1f)]
        [SerializeField]
        private float _attackTransitionProbability = 0.5f;

        private bool HasSelectableElements =>
            _nextStateTable == null || _nextStateTable?.Count > 0;

        protected void TransitionToNextState(LeviathanController owner)
        {
            if (UnityEngine.Random.value < _attackTransitionProbability || !HasSelectableElements)
            {
                owner.AttackStateSelector.GetRandomState().ChangeState(owner);
            }
            else
            {
                _nextStateTable.GetRandomStateType().ChangeState(owner);
            }
        }
    }
}