using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
        private Helper.OverLapSphere _groundChecker = default;
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
        public Helper.OverLapSphere GroundChecker => _groundChecker;
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
            _groundChecker.Enter();
            _stateMachine.Update();
            Move();
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
        [Header("アニメーションに関わる値")]
        [SerializeField]
        private GameObject _animationArrow = default;

        /// <summary> アニメーションの終了を検知する用の変数名 </summary>
        private AnimType _isAnimationEndDetection = AnimType.NotSet;
        /// <summary> 引数に指定されたアニメーションの再生が終了されるフレームのみ"true"を返す。 </summary>
        public bool IsAnimEnd(AnimType animType)
        {
            return _isAnimationEndDetection == animType;
        }
        /// <summary>
        /// 非同期で待つアニメーションの終了を待つ
        /// </summary>
        /// <param name="animType"> アニメーションの種類 </param>
        /// <param name="token"> キャンセル用トークン </param>
        /// <returns> 正常終了したとき true, キャンセル終了したとき flaseを返す。 </returns>
        public async UniTask<bool> IsAnimEndAsync(AnimType animType, CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => _isAnimationEndDetection == animType, cancellationToken: token);
            }
            catch (OperationCanceledException exception)
            {
                return false;
            }
            return true;
        }
        // アニメーションイベントで呼び出す想定で作成したメソッド
        /// <summary> 引数をフィールドに保存する。1フレーム後にフィールドを"AnimType.NotSet"に戻す。 </summary>
        public void OnAnimEnd(AnimType animType)
        {
            _isAnimationEndDetection = animType;
            if (animType == AnimType.Attack)
            {
                WaitAttackInterval();
            }
            Observable.NextFrame()
                .Subscribe(_ => _isAnimationEndDetection = AnimType.NotSet);
        }
        public void OnActiveWeapon()
        {
            if (_stateMachine.CurrentState is PlayerState05AttackBase)
            {
                (_stateMachine.CurrentState as PlayerState05AttackBase).Weapon.SetActive(true);
            }
            else
            {
                Debug.LogWarning("無効な命令が発行されました。");
            }
        }
        public void OnDisableWeapon()
        {
            // 遷移中なら無視する
            if (_animator.IsInTransition(1))
            {
                return;
            }
            if (_stateMachine.CurrentState is PlayerState05AttackBase)
            {
                (_stateMachine.CurrentState as PlayerState05AttackBase).Weapon.SetActive(false);
            }
            else
            {
                Debug.LogWarning("無効な命令が発行されました。");
            }
        }
        public void OnActiveArrow()
        {
            _animationArrow.SetActive(true);
        }
        public void OnDisableArrow()
        {
            _animationArrow.SetActive(false);
        }
        #endregion

        // ================== 移動に関連するメソッド群 ================== //
        #region Move
        #region Value
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
        #endregion

        #region Horizontal
        private Vector3 _moveSpeedHorizontal = default;
        private Quaternion _targetRotation = default;

        public bool CanMove { get => _canMove; set => _canMove = value; }
        public float MaxMoveSpeedHorizontal { get => _maxMoveSpeedHorizontal; set => _maxMoveSpeedHorizontal = value; }
        public float MoveHorizontalAcceleration { get => _movementAccelerationHorizontal; set => _movementAccelerationHorizontal = value; }

        private void Move()
        {
            Vector3 moveVector = MoveHorizontal();
            moveVector.y = MoveVertical();
            _characterController.Move(moveVector);
            // Debug.LogError(moveVector);
        }
        public void ResetMoveHorizontalSpeed()
        {
            _currentMoveSpeedHorizontal = 0f;
        }
        public Vector3 MoveHorizontal()
        {
            if (_canMove && Input.IsMoveInput)
            {
                // 現在、加減速の制御は Linear だが, 他の Easing を試す場合は、DOTweenを使用する。

                // 加速の計算
                _currentMoveSpeedHorizontal +=
                    _movementAccelerationHorizontal * Time.deltaTime;
                if (_currentMoveSpeedHorizontal > _maxMoveSpeedHorizontal)
                {
                    // Debug.Log("最大速度です");
                    _currentMoveSpeedHorizontal = _maxMoveSpeedHorizontal;
                }
                // 入力方向を取得
                _moveSpeedHorizontal = Input.MoveHorizontalDir.normalized;
                // メインカメラを基準に方向を指定する。
                _moveSpeedHorizontal = Camera.main.transform.TransformDirection(_moveSpeedHorizontal);
                if (_moveSpeedHorizontal.sqrMagnitude > 0.01f)
                {
                    _targetRotation = Quaternion.LookRotation(_moveSpeedHorizontal, Vector3.up);
                }
                _moveSpeedHorizontal.y = 0f;
                _moveSpeedHorizontal = _moveSpeedHorizontal.normalized;
                // 回転の抑制
                _targetRotation.x = 0f;
                _targetRotation.z = 0f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

                // 速度の適用
                _moveSpeedHorizontal *= _currentMoveSpeedHorizontal * Time.deltaTime;
            }
            else
            {
                // 減速の計算
                _currentMoveSpeedHorizontal -= _movementDecelerationHorizontal * Time.deltaTime;
                if (_currentMoveSpeedHorizontal < 0f) _currentMoveSpeedHorizontal = 0f;
                // 速度の適用
                _moveSpeedHorizontal = _moveSpeedHorizontal.normalized * _currentMoveSpeedHorizontal * Time.deltaTime;
            }
            // Debug.LogError(_moveSpeedHorizontal);
            return _moveSpeedHorizontal;
        }
        #endregion

        #region Vertical
        [SerializeField]
        private float _gravityOnGrounded = 1f;
        [SerializeField]
        private float _jumpInterval = 2f;
        [SerializeField]
        private bool _isVerticalCalculation = true;

        private bool _gravityOnGroundedGravity = true;
        private bool _isReadyJump = true;

        public bool IsReadyJump { get => _isReadyJump; set => _isReadyJump = value; }
        /// <summary> 垂直移動処理を実行するかどうかを表す値 </summary>
        public bool IsVerticalCalculation
        { get => _isVerticalCalculation; set => _isVerticalCalculation = value; }

        public float MoveVertical()
        {
            if (IsVerticalCalculation)
            {
                // Debug.LogError(GroundChecker.IsHit());
                // 垂直方向の移動計算
                if (!GroundChecker.IsHit() || !_gravityOnGroundedGravity) // 接地してない場合の処理
                {
                    _currentMoveSpeedVertical -= _gravity * Time.deltaTime; // 重力の計算
                }
                else // 接地している場合の処理
                {
                    _currentMoveSpeedVertical = _gravityOnGrounded * Time.deltaTime * -1f;
                }
                if (Input.IsJumpInput && GroundChecker.IsHit() && _isReadyJump) // ジャンプの処理
                {
                    _currentMoveSpeedVertical = _jumpSpeed;
                    StopOnGroundedGravity();
                }
            }
            else
            {
                _currentMoveSpeedVertical = 0f;
            }
            return _currentMoveSpeedVertical * Time.deltaTime;
        }
        /// <summary>
        /// 接地用重力計算をジャンプアニメーションが完了するまで停止する。
        /// </summary>
        private async void StopOnGroundedGravity()
        {
            _gravityOnGroundedGravity = false;

            // 条件を満たすまで待機
            // （ジャンプアニメーションが完了するまで重力の計算は通常通り行う）
            await UniTask.WaitUntil(() => IsAnimEnd(AnimType.Jump));

            _gravityOnGroundedGravity = true;
        }
        /// <summary>
        /// シリアライズフィールドで指定された値(_jumpInterval)時間ジャンプを実行できないようにする。
        /// </summary>
        private async void WaitJump()
        {
            _isReadyJump = false;
            await UniTask.Delay((int)(_jumpInterval * 1000f));
            _isReadyJump = true;
        }
        #endregion
        #endregion

        // ================== 攻撃に関連するメソッド群 ================== //
        #region Attack
        [Header("攻撃に関する値")]
        [SerializeField]
        private GameObject _attackArrow = default;
        [SerializeField]
        private GameObject _nomalArrow = default;
        [SerializeField]
        private Collider[] _nonContactTarget = default;
        [SerializeField]
        private Transform _arrowGeneratePos = default;
        [SerializeField]
        private float _minArrowShootPower = default;
        [SerializeField]
        private float _maxArrowShootPower = default;

        /// <summary>
        /// 攻撃入力を有効化, 無効化の設定する。<br/>
        /// 通常は, 攻撃ステート開始時と, 攻撃のアニメーションイベントから呼び出す。
        /// </summary>
        public void SetAcceptingAttackInput(bool value)
        {
            Input.IsAcceptingAttackInput = value;
        }
        /// <summary> 
        /// 指定された時間 攻撃入力を無効化する。<br/>
        /// 通常は, 攻撃ステートのExitメソッドで呼び出す。
        /// </summary>
        public async void WaitAttackInterval()
        {
            Input.IsAcceptingAttackInput = false;
            await UniTask.Delay((int)(_stateMachine.AttackStateController.AttackInterval * 1000f));
            Input.IsAcceptingAttackInput = true;
        }
        /// <summary>
        /// 矢を放つ処理
        /// </summary>
        public void ShootArrow()
        {
            // 下限値を加算した値を取得
            var shootPower = _minArrowShootPower +
                _stateMachine.AttackStateController.
                AttackState01BasicBow.CurrentShootArrowPower;
            // 上限値以内に補正する
            if (shootPower > _maxArrowShootPower)
                shootPower = _maxArrowShootPower;

            Debug.Log($"矢の力は\"{shootPower}\"です");
            // 矢を生成し、前方に放つ
            var arrow = Instantiate(_attackArrow, _arrowGeneratePos.position, Quaternion.identity);
            arrow.GetComponent<AttackArrowController>().Setup(_nomalArrow, _nonContactTarget, shootPower,
                new Vector3(transform.forward.x, 0.2f, transform.forward.z));

            // アニメーション用の矢を非アクティブにする。
            _animationArrow.SetActive(false);
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