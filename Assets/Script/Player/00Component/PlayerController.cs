using System.Collections;
using System.Collections.Generic;
using UniRx;
//using 
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Input _input = default;
        [SerializeField]
        private PlayerStateMachine _stateMachine = default;
        [SerializeField]
        private Helper.OverLapBox _groundChecker = default;
        [SerializeField]
        private Helper.Raycast _talkChecker = default;

        private CharacterController _characterController = null;
        private Animator _animator = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Helper.OverLapBox GroundChecker => _groundChecker;
        public Helper.Raycast TalkChecker => _talkChecker;

        private void Start()
        {
            _stateMachine.Init(this);
            _groundChecker.Init(transform);
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            _stateMachine.Update();
        }

        // ================== ダメージに関連するメソッド群 ================== //
        [SerializeField]
        private bool _isGodMode = false;
        public bool IsGodMode => _isGodMode;
        public void Damage(float value, Vector3 knockBackDir, float knockBackPower, DamageType type)
        {
            if (!_isGodMode)
            {
                // 体力を減らす
                Debug.LogWarning("体力を減らす処理は未実装です。");
                // ノックバックする
                Debug.LogWarning("ノックバック処理は未実装です。");

                // ステート遷移処理
                switch (type)
                {
                    case DamageType.Big:
                        // Debug.Log("ビッグダメージを受けました。BigDamageStateに遷移します。");
                        _stateMachine.TransitionTo(_stateMachine.BigDamage);
                        break;
                    case DamageType.Middle:
                        // Debug.Log("ミドルダメージを受けました。MiddleDamageStateに遷移します。");
                        _stateMachine.TransitionTo(_stateMachine.MiddleDamage);
                        break;
                    case DamageType.Small:
                        // Debug.Log("スモールダメージを受けました。SmallDamageStateに遷移します。");
                        _stateMachine.TransitionTo(_stateMachine.SmallDamage);
                        break;
                }
            }
        }
        public void StartGodMode()
        {
            _isGodMode = true;
        }
        public void EndGodMode()
        {
            _isGodMode = false;
        }

        // ================== アニメーションに関連するメソッド群 ================== //
        /// <summary> アニメーションの終了を検知する用の変数名 </summary>
        private AnimType _isAnimationEndDetection = AnimType.NotSet;
        /// <summary> 指定されたアニメーションの再生が終了されるフレームのみ"true"を返す。 </summary>
        public bool IsAnimEnd(AnimType animType)
        {
            // 結果を返す為に一時的に保存する。
            var result = _isAnimationEndDetection == animType;

            return result;
        }
        // アニメーションイベントで呼び出す想定で作成したメソッド
        /// <summary> 引数をフィールドに保存する。1フレーム後にフィールドを"AnimType.NotSet"に戻す。 </summary>
        public void OnAnimEnd(AnimType animType)
        {
            // 引数をフィールドに保存する。
            _isAnimationEndDetection = animType;
            // 1フレーム後に値を初期化する。
            Observable.NextFrame()
                .Subscribe(_ => _isAnimationEndDetection = AnimType.NotSet);
        }
    }

    public enum DamageType
    {
        Big, Middle, Small
    }
    /// <summary>
    /// 終了を感知したいアニメーションの種類
    /// </summary>
    [System.Serializable]
    public enum AnimType
    {
        NotSet,
        Jump,
        StandUp,
        MiddleDamage,
        SmallDamage,
        Land,
    }
}