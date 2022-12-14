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

        public bool IsJumpInput => UnityEngine.Input.GetButtonDown(_jump);
        public bool IsAttack1InputButtonDown => UnityEngine.Input.GetButtonDown(_attack1);
        public bool IsAttack2InputButtonDown => UnityEngine.Input.GetButtonDown(_attack2);
        public bool IsAttack1InputButton => UnityEngine.Input.GetButton(_attack1);
        public bool IsAttack2InputButton => UnityEngine.Input.GetButton(_attack2);

        public bool IsMenuInput => UnityEngine.Input.GetButtonDown(_menu);
        public bool IsTalkInput => UnityEngine.Input.GetButtonDown(_talk);

        public bool IsMoveInput => MoveHorizontalDir.sqrMagnitude > 0.1f;

        public Vector3 MoveHorizontalDir =>
            new Vector3(
                UnityEngine.Input.GetAxisRaw(_moveHorizontal),
                0f,
                UnityEngine.Input.GetAxisRaw(_moveVertical)).normalized;
    }
}