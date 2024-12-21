using System;
using UnityEngine;

namespace Confront.Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public int ID;
        [Tooltip("このテーブルが設定されていない場合、EnemyDataのDefaultDropItemTableが使用される。")]
        public TextAsset UniqueDropItemTable;

        private DropItemData[] _dropItemData = null;
        public DropItemData[] DropItemData => _dropItemData ??= UniqueDropItemTable.LoadDropItemTable();

        public EnemyData Data { get; private set; }

        public string Name => Data.Name;
        public string Description => Data.Description;

        private static LayerMask _playerLayerMask;

        public static LayerMask PlayerLayerMask
        {
            get
            {
                if (_playerLayerMask == 0) InitializeLayerMask();
                return _playerLayerMask;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeLayerMask()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
        }

        protected virtual void Awake()
        {
            Data = EnemyManager.EnemySheet.GetData(ID);
        }

        protected static float DefaultCalculateDamage(float attackPower, float defense)
        {
            float defenseDamageFactor;
            if (defense >= 0) // 防御力が正の場合
            {
                defenseDamageFactor = 100 / (100 + defense);
            }
            else // 防御力が負の場合
            {
                defenseDamageFactor = 1 + (-1f * defense) / 100;
            }
            return attackPower * defenseDamageFactor;
        }
    }
}