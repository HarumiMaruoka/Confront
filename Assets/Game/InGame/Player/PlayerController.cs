using Confront.ActionItem;
using Confront.AttackUtility;
using Confront.Debugger;
using Confront.Equipment;
using Confront.ForgeItem;
using Confront.GUI;
using Confront.Input;
using Confront.Player.Combo;
using Confront.SaveSystem;
using Confront.Weapon;
using System;
using Unity.Cinemachine;
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
        // GUI
        public MenuController MenuController;
        public GameOverPanel GameOverPanel;
        // アクションアイテム
        public ActionItemInventory ActionItemInventory = new ActionItemInventory();
        public HotBar HotBar = new HotBar();
        // フォージアイテム
        public ForgeItemInventory ForgeItemInventory = new ForgeItemInventory();
        // 攻撃系
        public ComboTree AttackComboTree => _equippedWeapon?.Data?.ComboTree;
        public AttackStateMachine AttackStateMachine;
        // 武器
        public int DefaultWeaponID;
        private WeaponInstance _equippedWeapon;
        public WeaponInventory WeaponInventory = new WeaponInventory();
        public WeaponActivator WeaponActivator;
        // 装備
        public PlayerEquipped Equipped = new PlayerEquipped();
        // Unityコンポーネント
        public CinemachineVirtualCamera VirtualCamera;
        public Animator Animator;
        private CharacterController _characterController;
        // Utility
        public DamageCameraShaker DamageCameraShaker;

        public Vector3 PrevPosition;
        public Vector3 CurrentPosition;
        public Vector3 InitialScale;

        public GameObject Owner => gameObject; // IDamageable
        public float AttackPower => CharacterStats.AttackPower * _equippedWeapon.AttackPower;

        public WeaponInstance EquippedWeapon
        {
            get => _equippedWeapon;
            set
            {
                if (value == null)
                {
                    Debug.LogError("weapon is null");
                    return;
                }
                if (value.Data == null)
                {
                    Debug.LogError("weapon data is null");
                    return;
                }

                _equippedWeapon = value;
                WeaponActivator.ActivateWeapon(value.Data.ID);

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
                    EquippedWeapon = WeaponInstance.Create(DefaultWeaponID);//new WeaponInstance(DefaultWeaponID);
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
                DirectionController.CurrentDirection = value.Direction;
                StateMachine.ChangeState(value.PlayerStateType);
                HealthManager = value.HealthManager;
                MovementParameters.Velocity = value.Velocity;
                MovementParameters.GrabIntervalTimer = value.GrabIntervalTimer;
                MovementParameters.PassThroughPlatformDisableTimer = value.PassThroughPlatformDisableTimer;
                ActionItemInventory = value.ActionItemInventory;
                EquippedWeapon = value.EquippedWeapon;
                ForgeItemInventory = value.ForgeItemInventory;
                WeaponInventory = value.WeaponInventory;
                HotBar = value.HotBar;

                saveData.PlayerData = null; // 何度もロードしないようにするため
            }

            CharacterController.enabled = true;
        }

        private void Update()
        {
            if (StateMachine.CurrentState is Dead) return;
            if (MenuController.IsOpenedMenu) return;

            StateMachine.Update();
            if (CharacterController.enabled) Move(MovementParameters.Velocity * Time.deltaTime);
            Animator.SetFloat("RunSpeed", Mathf.Abs(MovementParameters.Velocity.x / MovementParameters.MaxSpeed));
            MovementParameters.TimerUpdate();

            if (IsJumpable) StateMachine.ChangeState<Jump>();

            if (StateMachine.CurrentState is not InAir &&
                StateMachine.CurrentState is not Grounded)
            {
                DirectionController.UpdateDirection(MovementParameters.Velocity);
            }

            {
                var prevCCEnabled = CharacterController.enabled;

                CharacterController.enabled = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                CharacterController.enabled = prevCCEnabled;
            }
        }

        public bool DisablePlatformCollisionRequest = false;

        private void LateUpdate()
        {
            StateMachine.LateUpdate();

            PrevPosition = CurrentPosition;
            CurrentPosition = transform.position + (Vector3)Sensor._groundCheckRayOffset2;
            if (ShouldHandlePlatformCollision() && !DisablePlatformCollisionRequest)
            {
                HandlePlatformCollision();
            }
            DisablePlatformCollisionRequest = false;
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
        /// <param name="point"></param>
        public void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point)
        {
            if (StateMachine.CurrentState is Dead or GroundDodge or BigDamage) return;

            var damage = DefaultCalculateDamage(attackPower, CharacterStats.Defense);
            HealthManager.Damage(damage);

            var damageType = CalculateDamageType(damageVector);

            damageVector /= CharacterStats.Weight;
            damageVector = Vector2.ClampMagnitude(damageVector, MovementParameters.MaxDamageVector);

            if (HealthManager.IsDead && !DebugParams.Instance.IsGodMode)
            {
                StateMachine.ChangeState<Dead>();
            }
            else
            {
                switch (damageType)
                {
                    case DamageType.Mini:
                        {
                            Animator.CrossFade("Mini Damage", 0.1f, 1);
                            DamageCameraShaker.MiniDamageCameraShake();
                            break;
                        }
                    case DamageType.Small:
                        {
                            var state = StateMachine.ChangeState<SmallDamage>();
                            state.DamageDirection = damageVector;
                            DamageCameraShaker.SmallDamageCameraShake();
                            break;
                        }
                    case DamageType.Big:
                        {
                            var state = StateMachine.ChangeState<BigDamage>();
                            DamageCameraShaker.BigDamageCameraShake();
                            state.DamageDirection = damageVector;
                            break;
                        }
                }
            }
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
                var hitPoint = _platformHitInfo.point + (Vector3)Sensor._groundCheckRayOffset2;
                CharacterController.enabled = false;
                transform.position = new Vector3(hitPoint.x, hitPoint.y, 0f);
                CharacterController.enabled = true;

                MovementParameters.Velocity = new Vector2(0f, 0f);
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
                Rotation = DirectionController.CurrentRotation,
                Direction = DirectionController.CurrentDirection,
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
                EquippedWeapon = EquippedWeapon,
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

                if (StateMachine.CurrentState is SmallDamage or BigDamage)
                {
                    return false;
                }

                if (PlayerInputHandler.InGameInput.Jump.triggered)
                {
                    if (DebugParams.Instance.IsInfiniteJump) return true;
                    if (MovementParameters.JumpCount >= MovementParameters.MaxJumpCount) return false;
                    return true;
                }

                return false;
            }
        }

        public void Move(Vector3 delta)
        {
            if (StateMachine.CurrentState is GroundDodge)
            {
                CharacterController.Move(delta);
                return;
            }

            var rayOrigin = transform.position + (Vector3)Sensor._frontCheckRayOffset;
            var raySize = Sensor._frontCheckBoxRayHalfSize;

            var radius = CharacterController.radius;
            var length = Mathf.Abs(delta.x) + radius;
            var rayDirection = DirectionController.CurrentDirection == Direction.Right ? Vector3.right : Vector3.left;

            var layerMask = Sensor.GroundLayerMask | Sensor.EnemyLayerMask;

            var isHit = Physics.BoxCast(rayOrigin, raySize, rayDirection, out var hitInfo, Quaternion.identity, length, layerMask);

            if (isHit)
            {
                var hitDistance = hitInfo.distance - radius;
                delta.x = hitDistance * rayDirection.x;
            }

            CharacterController.Move(delta);
        }
    }
}