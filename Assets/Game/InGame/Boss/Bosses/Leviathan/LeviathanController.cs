using Confront.GameUI;
using Confront.Utility;
using INab.Dissolve;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public class LeviathanController : BossBase
    {
        public Animator Animator;
        public StateMachine StateMachine;
        public AttackStateSelector AttackStateSelector;
        [NonSerialized]
        public Direction Direction = Direction.Left;
        [SerializeField]
        private Vector2 _movementBoundaryLeftTop = new Vector2(-10, 10);
        [SerializeField]
        private Vector2 _movementBoundaryRightBottom = new Vector2(10, -10);
        [SerializeField]
        private Dissolver _dissolver;

        public TMPro.TextMeshProUGUI DebugText;
        private AnimatorPauseHandler _animatorPauseHandler;

        public float DirectionSign => Direction == Direction.Right ? 1 : -1;

        protected override void Awake()
        {
            base.Awake();
            _animatorPauseHandler = new AnimatorPauseHandler(Animator);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animatorPauseHandler.Dispose();
        }

        private void Start()
        {
            StateMachine.Initialize(this);
            AttackStateSelector.Owner = this;
            AttackStateSelector.RefreshRegionCenters();
            StateMachine.Die.Dissolver = _dissolver;
        }

        private void OnValidate()
        {
            if (AttackStateSelector != null)
            {
                AttackStateSelector.Owner = this;
                AttackStateSelector.RefreshRegionCenters();
            }

            if (_movementBoundaryLeftTop.x > _movementBoundaryRightBottom.x)
            {
                _movementBoundaryRightBottom.x = _movementBoundaryLeftTop.x;
            }
            if (_movementBoundaryLeftTop.y < _movementBoundaryRightBottom.y)
            {
                _movementBoundaryRightBottom.y = _movementBoundaryLeftTop.y;
            }
        }

        protected override void Update()
        {
            if (MenuController.IsOpened) return;

            base.Update();
            StateMachine.Update();
            // 行動制限
            var x = Mathf.Clamp(transform.position.x, _movementBoundaryLeftTop.x, _movementBoundaryRightBottom.x);
            var y = Mathf.Clamp(transform.position.y, _movementBoundaryRightBottom.y, _movementBoundaryLeftTop.y);
            transform.position = new Vector3(x, y, 0);
            // デバッグ：現在のステートを表示
            DebugText.text = StateMachine.CurrentState.GetType().Name;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((_movementBoundaryLeftTop + _movementBoundaryRightBottom) / 2, _movementBoundaryLeftTop - _movementBoundaryRightBottom);

            if (AttackStateSelector != null) AttackStateSelector.OnDrawGizmos();
            if (StateMachine.Attack1) StateMachine.Attack1.DrawGizmos(transform);
            if (StateMachine.Attack2) StateMachine.Attack2.DrawGizmos(transform);
            if (StateMachine.AttackHard) StateMachine.AttackHard.DrawGizmos(transform);
            if (StateMachine.AttackSpecial) StateMachine.AttackSpecial.DrawGizmos(transform);
            if (StateMachine.Roar) StateMachine.Roar.DrawGizmos(transform);
        }

        protected override string CreateSaveData()
        {
            Debug.Log("未実装");
            return "";
        }

        protected override void Load(string saveData)
        {
            Debug.Log("未実装");
        }

        protected override void OnDie()
        {
            StateMachine.ChangeState<Die>();
        }
    }
}