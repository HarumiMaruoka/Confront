using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("操作に関連するクラス")]
        [SerializeField]
        private Input _input = default;
        [SerializeField]
        private PlayerStateMachine _stateMachine = default;
        [Header("各チェッカー")]
        [SerializeField]
        private Helper.OverLapBox _groundChecker = default;
        [SerializeField]
        private Helper.Raycast _talkChecker = default;

        private CharacterController _characterController = null;
        private Rigidbody _rigidbody = null;
        private Animator _animator = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;
        public Helper.OverLapBox GroundChecker => _groundChecker;
        public Helper.Raycast TalkChecker => _talkChecker;

        // ========================= Unityイベント ========================= //
        #region UnityMethod
        private void Start()
        {
            _stateMachine.Init(this);
            _groundChecker.Init(transform);
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            ChangeMovementMethod(MovementMethodType.CharacterController);
        }
        private void Update()
        {
            _stateMachine.Update();
        }
        #endregion

        // ================== ダメージに関連するメソッド群 ================== //
        #region Damage
        [Header("ダメージに関連する値")]
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
                StartGodMode(); // ゴッドモードを起動する
                ChangeMovementMethod(MovementMethodType.Rigidbody);
                _rigidbody.AddForce(knockBackDir.normalized * knockBackPower, ForceMode.Impulse);

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
        /// <summary> 移動方法の変更 </summary>
        public void ChangeMovementMethod(MovementMethodType useType)
        {
            if (useType == MovementMethodType.CharacterController)
            {
                _rigidbody.isKinematic = true;       // 物理計算を無効化する
                _characterController.enabled = true; // キャラクターコントローラーを起動する
            }
            else if (useType == MovementMethodType.Rigidbody)
            {
                _rigidbody.isKinematic = false;       // 物理計算を有効化する
                _characterController.enabled = false; // キャラクターコントローラーを停止する
            }
            else
            {
                Debug.LogError("想定していない値が渡されました");
            }
        }
        public enum MovementMethodType
        {
            CharacterController,
            Rigidbody,
        }
        #endregion

        // ================== アニメーションに関連するメソッド群 ================== //
        #region Animation
        /// <summary> アニメーションの終了を検知する用の変数名 </summary>
        private AnimType _isAnimationEndDetection = AnimType.NotSet;
        /// <summary> 引数に指定されたアニメーションの再生が終了されるフレームのみ"true"を返す。 </summary>
        public bool IsAnimEnd(AnimType animType)
        {
            return _isAnimationEndDetection == animType;
        }
        // アニメーションイベントで呼び出す想定で作成したメソッド
        /// <summary> 引数をフィールドに保存する。1フレーム後にフィールドを"AnimType.NotSet"に戻す。 </summary>
        public void OnAnimEnd(AnimType animType)
        {
            _isAnimationEndDetection = animType;
            Observable.NextFrame()
                .Subscribe(_ => _isAnimationEndDetection = AnimType.NotSet);
        }
        #endregion

        // ================== 移動に関連するメソッド群 ================== //
        #region Move
        [Header("移動に関連する値")]
        [SerializeField]
        [Tooltip("テスト用移動速度 : ステータス制御が完成したとき削除する値")]
        private float _testHorizontalMoveSpeed = 4;
        [SerializeField]
        private float _jumpSpeed = 0.8f;
        [SerializeField]
        private float _gravity = 0.5f;
        [SerializeField]
        private float _rotationSpeed = 600f;
        [SerializeField]
        private bool _canMove = true;

        private Vector3 _moveSpeed = default;
        private float _moveSpeedY = 0f;
        Quaternion _targetRotation = default;
        public void Move()
        {
            if (_canMove)
            {
                // 水平方向の移動計算
                _moveSpeed = Input.MoveHorizontalDir.normalized;
                //　メインカメラを基準に方向を決める。
                _moveSpeed = Camera.main.transform.TransformDirection(_moveSpeed);
                // 移動方向を向く
                if (_moveSpeed.magnitude > 0.5f)
                {
                    _targetRotation = Quaternion.LookRotation(_moveSpeed, Vector3.up);
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                var rotation = transform.rotation;
                rotation.x = 0f;
                rotation.z = 0f;
                transform.rotation = rotation;
                _moveSpeed *= _testHorizontalMoveSpeed * Time.deltaTime;
            }

            // 垂直方向の移動計算
            if (!GroundChecker.IsHit()) // 接地してない場合の処理
            {
                _moveSpeedY -= _gravity * Time.deltaTime; // 重力の計算
                _moveSpeed.y = _moveSpeedY;
            }
            else // 接地している場合の処理
            {
                _moveSpeed.y = _moveSpeedY = 0f;
            }

            if (Input.IsJumpInput && GroundChecker.IsHit()) // ジャンプの処理
            {
                _moveSpeedY = _jumpSpeed;
            }

            // キャラクターコントローラーに値を渡す。
            _characterController.Move(_moveSpeed);
        }
        #endregion
    }
    #region Enum
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
    #endregion
}