using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState07Talk : PlayerState00Base
    {
        private Human.HumanController _human = null;
        public override void Enter()
        {
            if (_stateMachine.PlayerController.TalkChecker.Result.collider.TryGetComponent(out Human.HumanController human))
            {
                _human = human;

                _human?.TalkBehaviour.StartTalk();
            }
        }
        public override void Exit()
        {
            _human?.TalkBehaviour.Exit();
        }
        [SerializeField]
        private float _exitDistance = 8f;
        public override void Update()
        {
            float sqrDistance = (_stateMachine.PlayerController.transform.position - _human.transform.position).sqrMagnitude;
            // 何かの影響でTalkHumanとの距離が離れすぎた場合, あるいは
            // 会話アルゴリズムが完了したとき、状態を遷移する。
            if (sqrDistance > _exitDistance * _exitDistance ||
                _human.TalkBehaviour.IsComplete)
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
                // 移動入力があるとき、ステートをMoveに遷移する。
                if (_stateMachine.PlayerController.Input.IsMoveInput)
                {
                    _stateMachine.TransitionTo(_stateMachine.Move);
                    return;
                }
                // 移動入力があるとき、ステートをMoveに遷移する。
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.Idle);
                    return;
                }
            }
        }
    }
}