namespace Player
{
    [System.Serializable]
    public class AttackState00BasicGunFire : PlayerState05AttackBase
    {
        public override void Update()
        {
            // 構えアニメーションが完了したらFireアニメーションを再生する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.HoldWeapon))
            {
                UpdteComboAnim();
            }
            // Fireアニメーションの完了時に
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // 攻撃入力が無ければ, 武器を構えを解くアニメーションを再生する。
                if (!_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                    !_stateMachine.PlayerController.Input.IsAttack2InputButton())
                {
                    UpdteComboAnim();
                }
                // そうでなければ, 武器を構えを解くアニメーションを再生する。
                // （アニメーションは, ループ再生で表現するので特に何もしない。）
            }
            // 武器の構えを解くアニメーションの再生が完了したら, 他のステートに遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
                // 着地アニメーションの再生が終了したとき遷移処理を実行する。
                if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Land))
                {
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
}