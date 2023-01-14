using System.Collections;
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
        [Header("ステータス管理")]
        [SerializeField]
        private PlayerStatusManager _playerStatusManager = default;

        private CharacterController _characterController = null;
        private Rigidbody _rigidbody = null;
        private Animator _animator = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public PlayerStatusManager PlayerStatusManager => _playerStatusManager;
        public CharacterController CharacterController => _characterController;
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;
        public Helper.OverLapBox GroundChecker => _groundChecker;
        public Helper.Raycast TalkChecker => _talkChecker;

        // ========================= Unityイベント ========================= //
        #region UnityMethod
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
            _stateMachine.Init(this);
            _groundChecker.Init(transform);
            _playerStatusManager.LoadData();
            ChangeMovementMethod(MovementMethodType.CharacterController);
        }
        private void Update()
        {
            _stateMachine.Update();
            MoveHorizontal();
            MoveVertical();
        }
        private void OnDrawGizmosSelected()
        {
            _groundChecker.OnDrawGizmos(transform);
            _talkChecker.OnDrawGizmo(transform);
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
            if (animType == AnimType.Attack)
            {
                StartCoroutine(WaitAttackInterval());
            }
            Observable.NextFrame()
                .Subscribe(_ => _isAnimationEndDetection = AnimType.NotSet);
        }
        /// <summary> 攻撃入力を有効化, 無効化の設定する </summary>
        public void SetAcceptingAttackInput(bool value)
        {
            Input.IsAcceptingAttackInput = value;
        }
        /// <summary> 指定された時間 攻撃入力を無効化する </summary>
        private IEnumerator WaitAttackInterval()
        {
            Input.IsAcceptingAttackInput = false;
            yield return new WaitForSeconds(_stateMachine.AttackStateController.AttackInterval);
            Input.IsAcceptingAttackInput = true;
        }
        #endregion

        // ================== 移動に関連するメソッド群 ================== //
        #region Move
        [Header("移動に関連する値")]

        [Tooltip("移動速度の最大値")]
        [SerializeField]
        private float _maxMoveSpeedHorizontal = 4f;
        [Tooltip("移動加速度")]
        [SerializeField]
        private float _movementAccelerationHorizontal = 0.4f;
        [Tooltip("移動減速度")]
        [SerializeField]
        private float _movementDecelerationHorizontal = 0.4f;
        [Tooltip("現在のプレイヤーの水平移動速度を表す値")]
        [SerializeField]
        private float _currentMoveSpeedHorizontal = 0f;
        [Tooltip("現在のプレイヤーの垂直移動速度を表す値")]
        [SerializeField]
        private float _currentMoveSpeedVertical = 0f;

        [SerializeField]
        private float _jumpSpeed = 0.8f;
        [SerializeField]
        private float _gravity = 0.5f;
        [SerializeField]
        private float _rotationSpeed = 600f;
        [SerializeField]
        private bool _canMove = true;

        public float SpecialAcceleration { get; set; } = 1f;
        public bool CamMove { get => _canMove; set => _canMove = value; }

        private Vector3 _moveSpeedHorizontal = default;
        Quaternion _targetRotation = default;

        public void ResetMoveHorizontalSpeed()
        {
            _currentMoveSpeedHorizontal = 0f;
        }
        public void MoveHorizontal()
        {
            if (_canMove && Input.IsMoveInput)
            {
                // 加速の計算
                _currentMoveSpeedHorizontal +=
                    _movementAccelerationHorizontal * Time.deltaTime * SpecialAcceleration;
                if (_currentMoveSpeedHorizontal > _maxMoveSpeedHorizontal)
                {
                    _currentMoveSpeedHorizontal = _maxMoveSpeedHorizontal;
                }
                // 入力方向を取得
                _moveSpeedHorizontal = Input.MoveHorizontalDir.normalized;
                // メインカメラを基準に方向を指定する。
                _moveSpeedHorizontal = Camera.main.transform.TransformDirection(_moveSpeedHorizontal);
                if (_moveSpeedHorizontal.magnitude > 0.5f)
                {
                    _targetRotation = Quaternion.LookRotation(_moveSpeedHorizontal, Vector3.up);
                }
                // 回転の抑制
                _targetRotation.x = 0f;
                _targetRotation.z = 0f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                // 速度の適用
                _moveSpeedHorizontal *= _currentMoveSpeedHorizontal;
            }
            else
            {
                // 減速の計算
                _currentMoveSpeedHorizontal -= _movementDecelerationHorizontal * Time.deltaTime;
                if (_currentMoveSpeedHorizontal < 0f) _currentMoveSpeedHorizontal = 0f;
                // 速度の適用
                _moveSpeedHorizontal *= _currentMoveSpeedHorizontal * Time.deltaTime;
            }
            _moveSpeedHorizontal.y = 0f;
            // キャラクターコントローラーに値を渡す。
            _characterController.Move(_moveSpeedHorizontal);
        }

        [SerializeField]
        private float _gravityOnGrounded = 1f;
        [SerializeField]
        private float _jumpInterval = 2f;

        private bool _gravityOnGroundedGravity = default;
        private bool _isReadyJump = true;

        public bool IsReadyJump => _isReadyJump;

        [SerializeField]
        private bool _isVerticalCalculation = true;
        /// <summary> 垂直移動処理を実行するかどうかを表す値 </summary>
        public bool IsVerticalCalculation
        { get => _isVerticalCalculation; set => _isVerticalCalculation = value; }
        public void MoveVertical()
        {
            if (IsVerticalCalculation)
            {
                // 垂直方向の移動計算
                if (!GroundChecker.IsHit() || !_gravityOnGroundedGravity) // 接地してない場合の処理
                {
                    _currentMoveSpeedVertical -= _gravity * Time.deltaTime; // 重力の計算
                }
                else if (_gravityOnGroundedGravity) // 接地している場合の処理
                {
                    _currentMoveSpeedVertical = _gravityOnGrounded * Time.deltaTime * -1f;
                }
                if (Input.IsJumpInput && GroundChecker.IsHit() && _isReadyJump) // ジャンプの処理
                {
                    _currentMoveSpeedVertical = _jumpSpeed;
                    StartCoroutine(StopOnGroundedGravity());
                    StartCoroutine(WaitJump());
                }
                // キャラクターコントローラーに値を渡す。
                _characterController.Move(new Vector3(0f, _currentMoveSpeedVertical) * Time.deltaTime);
            }
        }
        private IEnumerator StopOnGroundedGravity()
        {
            _gravityOnGroundedGravity = false;

            while (!IsAnimEnd(AnimType.Jump))
            {
                yield return null;
            }
            _gravityOnGroundedGravity = true;
        }
        private IEnumerator WaitJump()
        {
            _isReadyJump = false;
            yield return new WaitForSeconds(_jumpInterval);
            _isReadyJump = true;
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

        /// <summary> 武器を構える </summary>
        HoldWeapon,
        /// <summary> 攻撃 </summary>
        Attack,
        /// <summary> 武器の構えを解く </summary>
        UnarmWeapon,
    }
    #endregion
}