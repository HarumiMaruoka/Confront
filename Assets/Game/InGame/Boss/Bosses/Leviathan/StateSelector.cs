using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/StateSelector")]
    public class StateSelector : ScriptableObject
    {
        public LeviathanController Owner;
        public StateRegion[] Regions;
        public StateTable PlayerOutOfBoundsTable; // Playerが範囲外にいるときのStateTable

        public IState GetRandomState()
        {
            if (Owner == null)
            {
                Debug.LogWarning("Owner is null");
                return null;
            }
            if (Regions == null || Regions.Length == 0)
            {
                Debug.LogWarning("Regions is null or empty");
                return null;
            }
            var player = PlayerController.Instance;
            if (player == null)
            {
                Debug.LogWarning("PlayerController is null");
                return null;
            }

            foreach (var region in Regions)
            {
                if (region.Contains(player.transform.position))
                {
                    return region.GetRandomState().ToState(Owner);
                }
            }
            return PlayerOutOfBoundsTable.GetRandomState().ToState(Owner);
        }

        public void OnDrawGizmos()
        {
            if (Owner == null)
            {
                Debug.LogWarning("Owner is null");
                return;
            }
            if (Regions == null || Regions.Length == 0)
            {
                Debug.LogWarning("Regions is null or empty");
                return;
            }

            foreach (var region in Regions)
            {
                region.Center = Owner.transform;
                region.OnDrawGizmos();
            }
        }
    }

    [Serializable]
    public class StateRegion
    {
        public Transform Center;
        public StateTable Table;
        public Vector2 LocalTopLeft;
        public Vector2 LocalBottomRight;

        public Vector2 GlobalTopLeft => new Vector2(Center.position.x + LocalTopLeft.x, Center.position.y + LocalTopLeft.y);
        public Vector2 GlobalBottomRight => new Vector2(Center.position.x + LocalBottomRight.x, Center.position.y + LocalBottomRight.y);

        public bool Contains(Vector2 position)
        {
            return GlobalTopLeft.x <= position.x && position.x <= GlobalBottomRight.x &&
                   GlobalTopLeft.y <= position.y && position.y <= GlobalBottomRight.y;
        }

        public StateType GetRandomState()
        {
            return Table.GetRandomState();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(GlobalTopLeft, new Vector2(GlobalBottomRight.x, GlobalTopLeft.y)); // 上辺
            Gizmos.DrawLine(GlobalTopLeft, new Vector2(GlobalTopLeft.x, GlobalBottomRight.y)); // 左辺
            Gizmos.DrawLine(new Vector2(GlobalBottomRight.x, GlobalTopLeft.y), GlobalBottomRight); // 右辺
            Gizmos.DrawLine(new Vector2(GlobalTopLeft.x, GlobalBottomRight.y), GlobalBottomRight); // 下辺
        }
    }

    [Serializable]
    public class StateTable
    {
        public WeightedState[] Table;

        public StateTable(WeightedState[] table)
        {
            Table = table;
        }

        public StateType GetRandomState()
        {
            if (Table == null || Table.Length == 0)
            {
                throw new Exception("Table is null or empty");
            }

            var totalWeight = 0f;
            foreach (var state in Table)
            {
                totalWeight += state.Weight;
            }
            var randomValue = UnityEngine.Random.Range(0f, totalWeight);
            foreach (var state in Table)
            {
                randomValue -= state.Weight;
                if (randomValue <= 0f)
                {
                    return state.State;
                }
            }
            return Table[0].State;
        }
    }

    [Serializable]
    public struct WeightedState
    {
        public float Weight;
        public StateType State;
    }

    public enum StateType
    {
        Idle,
        Walk,
        Stunned,
        GetHit1,
        GetHit2,
        Die,
        Attack1,
        Attack2,
        AttackHard,
        AttackSpecial,
        Roar,
        Block,
    }

    public static class StateTypeExtensions
    {
        public static IState ToState(this StateType stateType, LeviathanController leviathan)
        {
            switch (stateType)
            {
                case StateType.Idle: return leviathan.StateMachine.Idle;
                case StateType.Walk: return leviathan.StateMachine.Walk;
                case StateType.Stunned: return leviathan.StateMachine.Stunned;
                case StateType.GetHit1: return leviathan.StateMachine.GetHit1;
                case StateType.GetHit2: return leviathan.StateMachine.GetHit2;
                case StateType.Die: return leviathan.StateMachine.Die;
                case StateType.Attack1: return leviathan.StateMachine.Attack1;
                case StateType.Attack2: return leviathan.StateMachine.Attack2;
                case StateType.AttackHard: return leviathan.StateMachine.AttackHard;
                case StateType.AttackSpecial: return leviathan.StateMachine.AttackSpecial;
                case StateType.Roar: return leviathan.StateMachine.Roar;
                case StateType.Block: return leviathan.StateMachine.Block;
                default: throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }
    }
}