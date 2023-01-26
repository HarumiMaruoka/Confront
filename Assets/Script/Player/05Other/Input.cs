using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Input
    {
        [InputName, SerializeField]
        private string _moveHorizontal = default;
        [InputName, SerializeField]
        private string _moveVertical = default;
        [InputName, SerializeField]
        private string _jump = default;

        [InputName, SerializeField]
        private string _attack1 = default;
        [InputName, SerializeField]
        private string _attack2 = default;

        [InputName, SerializeField]
        private string _menu = default;
        [InputName, SerializeField]
        private string _talk = default;

        public string TalkButtonName => _talk;

        public bool IsAcceptingAttackInput { get; set; } = true;

        // ジャンプ
        public bool IsJumpInput => UnityEngine.Input.GetButtonDown(_jump);
        // メニュー
        public bool IsMenuInput => UnityEngine.Input.GetButtonDown(_menu);
        // 会話
        public bool IsTalkInput => UnityEngine.Input.GetButtonDown(_talk);
        // 移動
        public bool IsMoveInput => MoveHorizontalDir.sqrMagnitude > 0.1f;
        public Vector3 MoveHorizontalDir =>
            new Vector3(
                UnityEngine.Input.GetAxisRaw(_moveHorizontal),
                0f,
                UnityEngine.Input.GetAxisRaw(_moveVertical)).normalized;
        // 攻撃
        public bool IsAttack1InputButtonDown()
        {
            if (IsAcceptingAttackInput)
                return UnityEngine.Input.GetButtonDown(_attack1);
            else
                return false;
        }
        public bool IsAttack2InputButtonDown()
        {
            if (IsAcceptingAttackInput)
                return UnityEngine.Input.GetButtonDown(_attack2);
            else
                return false;
        }
        public bool IsAttack1InputButton()
        {
            if (IsAcceptingAttackInput)
                return UnityEngine.Input.GetButton(_attack1);
            else
                return false;
        }
        public bool IsAttack2InputButton()
        {
            if (IsAcceptingAttackInput)
                return UnityEngine.Input.GetButton(_attack2);
            else
                return false;
        }
    }
}