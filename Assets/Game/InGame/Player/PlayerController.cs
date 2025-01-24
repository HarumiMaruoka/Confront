using Cinemachine;
using Confront.ActionItem;
using Confront.AttackUtility;
using Confront.ForgeItem;
using Confront.GameUI;
using Confront.Input;
using Confront.Player.Combo;
using Confront.SaveSystem;
using Confront.Weapon;
using System;
using UnityEngine;

namespace Confront.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, ISavable, IDamageable
    {
        public static PlayerController Instance { get; private set; }

        private void OnValidate()
        {
            Instance = this;
            Debug.Log("PlayerController is set to Instance.");
        }

        private void Awake()
        {
            Instance = this;
            SavableRegistry.Register(this);
            Initialize(SaveDataController.Loaded);

            PrevPosition = transform.position;
            CurrentPosition = transform.position;
        }

        // インゲーム制御
        public StateMachine StateMachine;
        // 移動系
        public MovementParameters MovementParameters;
        public Sensor Sensor;
        public DirectionController DirectionController;
        // ステータス系
        public CharacterStats CharacterStats;
        public HealthManager HealthManager;
        // メニュー
        public MenuController MenuController;
        // アクションアイテム
        public ActionItemInventory ActionItemInventory = new ActionItemInventory();
        public HotBar HotBar = new HotBar();
        // フォージアイテム
        public ForgeItemInventory ForgeItemInventory = new ForgeItemInventory();
        // 攻撃系
        public ComboTree AttackComboTree => _equippedWeapon?.Data.ComboTree;
        public AttackStateMachine AttackStateMachine;
        // 武器
        public int DefaultWeaponID;
        private WeaponInstance _equippedWeapon;
        public WeaponInventory WeaponInventory = new WeaponInventory();
        public WeaponActivator WeaponActivator;
        // Unityコンポーネント
        public CinemachineVirtualCamera VirtualCamera;
        public Animator Animator;
        private CharacterController _characterController;

        public Vector3 PrevPosition;
        public Vector3 CurrentPosition;
        public Vector3 InitialScale;

        public WeaponInstance EquippedWeapon
        {
            get => _equippedWeapon;
            set
            {
                _equippedWeapon = value;
                if (value != null) WeaponActivator.ActivateWeapon(value.Data.ID);

                OnWeaponEquipped?.Invoke(value);
            }
        }
        public event Action<WeaponInstance> OnWeaponEquipped;

        public CharacterController CharacterController
        {
            get
            {
                if (!_characterController) _characterController = GetComponent<CharacterController>();
                return _characterController;
            }
        }

        private bool _isInitialized = false;

        public void Initialize(SaveData saveData = null)
        {
            CharacterController.enabled = false;
            if (!_isInitialized)
            {
                StateMachine = new StateMachine(this);
                StateMachine.ChangeState<Grounded>();
                DirectionController.Initialize(transform);
                AttackStateMachine = new AttackStateMachine();
                HealthManager = new HealthManager(CharacterStats.MaxHealth, CharacterStats.MaxHealth);
                _isInitialized = true;
                InitialScale = transform.localScale;

                if (EquippedWeapon == null)
                {
                    EquippedWeapon = new WeaponInstance(DefaultWeaponID);
                    WeaponInventory.AddWeapon(EquippedWeapon);
                }

                MenuController.OnOpenedMenu += OnOpenedMenu;
                MenuController.OnClosedMenu += OnClosedMenu;
            }
            if (saveData != null && saveData.PlayerData.HasValue)
            {
                var value = saveData.PlayerData.Value;
                transform.position = value.Position;
                transform.rotation = value.Rotation;
                StateMachine.ChangeState(value.PlayerStateType);
                HealthManager = value.HealthManager;
                MovementParameters.Velocity = value.Velocity;
                MovementParameters.GrabIntervalTimer = value.GrabIntervalTimer;
                MovementParameters.PassThroughPlatformDisableTimer = value.PassThroughPlatformDisableTimer;
                ActionItemInventory = value.ActionItemInventory;
                ForgeItemInventory = value.ForgeItemInventory;
                _equippedWeapon = value.EquippedWeapon;
                WeaponInventory = value.WeaponInventory;
                HotBar = value.HotBar;

                saveData.PlayerData = null; // 何度もロードしないようにするため
            }

            CharacterController.enabled = true;
        }

        private void Update()
        {
            if (MenuController.IsOpenedMenu) return;

            StateMachine.Update();
            if (CharacterController.enabled) CharacterController.Move(MovementParameters.Velocity * Time.deltaTime);
            Animator.SetFloat("RunSpeed", Mathf.Abs(MovementParameters.Velocity.x / MovementParameters.MaxSpeed));
            MovementParameters.TimerUpdate();

            if (IsJumpable) StateMachine.ChangeState<Jump>();

            if (StateMachine.CurrentState is not InAir &&
                StateMachine.CurrentState is not Grounded)
            {
                DirectionController.UpdateVelocity(MovementParameters.Velocity);
            }

            {
                var prevCCEnabled = CharacterController.enabled;

                CharacterController.enabled = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                CharacterController.enabled = prevCCEnabled;
            }
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        private void LateUpdate()
        {
            StateMachine.LateUpdate();

            PrevPosition = CurrentPosition;
            CurrentPosition = transform.position + (Vector3)Sensor._groundCheckRayOffset;
            if (ShouldHandlePlatformCollision())
            {
                HandlePlatformCollision();
            }
        }

        private bool ShouldHandlePlatformCollision()
        {
            var isDamageStateMovingUp = StateMachine.CurrentState is BigDamage or SmallDamage && MovementParameters.Velocity.y > 0f;

            var isInvalidState = StateMachine.CurrentState is Grab or Jump;

            return !isInvalidState && !isDamageStateMovingUp;
        }

        /// <summary>
        /// プレイヤーにダメージを与える
        /// </summary>
        /// <param name="attackPower"> 最終攻撃ダメージ </param>
        /// <param name="damageVector"> ノックバックに使用する </param>
        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            if (StateMachine.CurrentState is GroundDodge or BigDamage) return;

            var damage = DefaultCalculateDamage(attackPower, CharacterStats.Defense);
            HealthManager.Damage(damage);

            var damageType = CalculateDamageType(damageVector);

            damageVector /= CharacterStats.Weight;
            damageVector = Vector2.ClampMagnitude(damageVector, MovementParameters.MaxDamageVector);

            //if (HealthManager.IsDead)
            //{
            //    // StateMachine.ChangeState<Dead>();
            //}
            //else
            //{
            switch (damageType)
            {
                case DamageType.Mini:
                    {
                        Animator.CrossFade("Mini Damage", 0.1f, 1);
                        break;
                    }
                case DamageType.Small:
                    {
                        var state = StateMachine.ChangeState<SmallDamage>();
                        state.DamageDirection = damageVector;
                        break;
                    }
                case DamageType.Big:
                    {
                        var state = StateMachine.ChangeState<BigDamage>();
                        state.DamageDirection = damageVector;
                        break;
                    }
            }
            //}
        }

        public enum DamageType
        {
            Mini,
            Small,
            Big,
        }

        private DamageType CalculateDamageType(Vector2 damageVector)
        {
            var magnitude = damageVector.magnitude;
            var factor = magnitude / CharacterStats.Weight;

            if (factor < 5f) return DamageType.Mini;
            if (factor < 15f && StateMachine.CurrentState is not SmallDamage) return DamageType.Small;
            return DamageType.Big;
        }

        private float DefaultCalculateDamage(float attackPower, float defense)
        {
            float defenseDamageFactor;
            if (defense > 0)
            {
                defenseDamageFactor = 100 / (100 + defense);
            }
            else
            {
                defenseDamageFactor = 1 + (defense * -1) / 100;
            }

            var damage = attackPower * defenseDamageFactor;
            return damage;
        }

        private RaycastHit _platformHitInfo; // Gizmoを描画するために保持

        public void HandlePlatformCollision()
        {
            if (MovementParameters.IsPassThroughPlatformTimerFinished &&
                Physics.Linecast(PrevPosition, CurrentPosition, out _platformHitInfo, Sensor.PassThroughPlatform | Sensor.GroundLayerMask))
            {
                var hitPoint = _platformHitInfo.point + (Vector3)Sensor._groundCheckRayOffset;
                CharacterController.enabled = false;
                transform.position = new Vector3(hitPoint.x, hitPoint.y, 0f);
                CharacterController.enabled = true;

                MovementParameters.Velocity = new Vector2(0f, 0f);
                StateMachine.ChangeState<Grounded>();
            }
        }

        private void OnDestroy()
        {
            SavableRegistry.Unregister(this);
            if (MenuController)
            {
                MenuController.OnOpenedMenu -= OnOpenedMenu;
                MenuController.OnClosedMenu -= OnClosedMenu;
            }
            Instance = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Sensor) Sensor.DrawGizmos(this);

            // PrevPositionからCurrentPositionまでの線を描画
            Gizmos.color = _platformHitInfo.collider ? Color.red : Color.blue;
            Gizmos.DrawLine(PrevPosition, CurrentPosition);
        }
#endif

        private void OnOpenedMenu()
        {
            Animator.StartPlayback();
        }

        private void OnClosedMenu()
        {
            Animator.StopPlayback();
        }

        public void Save(SaveData saveData)
        {
            saveData.PlayerData = new PlayerData
            {
                // Transform
                Position = transform.position,
                Rotation = transform.rotation,
                // StateMachine
                PlayerStateType = StateMachine.CurrentState.GetType(),
                // MovementParameters
                Velocity = MovementParameters.Velocity,
                GrabIntervalTimer = MovementParameters.GrabIntervalTimer,
                PassThroughPlatformDisableTimer = MovementParameters.PassThroughPlatformDisableTimer,
                // HealthManager
                HealthManager = HealthManager,
                // Action Item
                ActionItemInventory = ActionItemInventory,
                HotBar = HotBar,
                // Forge Item
                ForgeItemInventory = ForgeItemInventory,
                // Weapon
                EquippedWeapon = _equippedWeapon,
                WeaponInventory = WeaponInventory,
            };
        }

        public void EquipWeapon(WeaponInstance instance)
        {
            EquippedWeapon = instance;
        }

        private bool IsJumpable
        {
            get
            {
                var leftStick = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
                if (leftStick.sqrMagnitude > 0.1f)
                {
                    var angle = Vector2.Angle(leftStick, Vector2.down);
                    if (PlayerInputHandler.InGameInput.Jump.triggered && angle < 45f)
                    {
                        return false;
                    }
                }

                if (PlayerInputHandler.InGameInput.Jump.triggered) return true;

                return false;
            }
        }
    }
}