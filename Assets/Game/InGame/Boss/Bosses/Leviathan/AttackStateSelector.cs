using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    // 距離に応じて攻撃パターンを選択する。
    [Serializable]
    public class AttackStateSelector
    {
        [HideInInspector]
        public LeviathanController Owner;
        public StateRegion[] Regions;
        public StateTable PlayerOutOfBoundsTable; // Playerが範囲外にいるときのStateTable

        public void RefreshRegionCenters()
        {
            if (Owner == null || Regions == null || Regions.Length == 0) return;

            foreach (var region in Regions)
            {
                region.Center = Owner.transform;
            }
        }

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
            return PlayerOutOfBoundsTable.GetRandomStateType().ToState(Owner);
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
            var player = PlayerController.Instance;
            if (player == null)
            {
                Debug.LogWarning("PlayerController is null");
                return;
            }

            bool previouslyContained = false;
            foreach (var region in Regions)
            {
                // 最初にPlayerが範囲内にいるときだけ赤色にする
                if (previouslyContained)
                {
                    Gizmos.color = Color.green;
                }
                else if (region.Contains(player.transform.position))
                {
                    Gizmos.color = Color.red;
                    previouslyContained = true;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                region.OnDrawGizmos();
            }
        }
    }

    [Serializable]
    public class StateRegion
    {
        public Vector2 LocalTopLeft;
        public Vector2 LocalBottomRight;

        [HideInInspector]
        public Transform Center;
        public StateTable Table;

        public Vector2 GlobalTopLeft => Center.rotation * new Vector3(0, LocalTopLeft.y, -LocalTopLeft.x) + Center.position;
        public Vector2 GlobalBottomRight => Center.rotation * new Vector3(0, LocalBottomRight.y, -LocalBottomRight.x) + Center.position;

        public bool Contains(Vector2 position)
        {
            var top = GlobalTopLeft.y >= GlobalBottomRight.y ? GlobalTopLeft.y : GlobalBottomRight.y;
            var bottom = GlobalTopLeft.y <= GlobalBottomRight.y ? GlobalTopLeft.y : GlobalBottomRight.y;
            var left = GlobalTopLeft.x <= GlobalBottomRight.x ? GlobalTopLeft.x : GlobalBottomRight.x;
            var right = GlobalTopLeft.x >= GlobalBottomRight.x ? GlobalTopLeft.x : GlobalBottomRight.x;

            return left <= position.x && position.x <= right &&
                   top >= position.y && position.y >= bottom;
        }

        public StateType GetRandomState()
        {
            return Table.GetRandomStateType();
        }

        public void OnDrawGizmos()
        {
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

        public int? Count => Table?.Length;

        public StateTable(WeightedState[] table)
        {
            Table = table;
        }

        public StateType GetRandomStateType()
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
}