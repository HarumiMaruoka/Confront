using Confront.DropItem;
using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public int ID;
        [Tooltip("このテーブルが設定されていない場合、EnemyDataのDefaultDropItemTableが使用される。")]
        public TextAsset UniqueDropItemTableInput;

        private DropItemData[] _dropItemTable = null;
        public DropItemData[] UniqueDropItemTable
        {
            get
            {
                if (UniqueDropItemTableInput == null) return null;
                return _dropItemTable ??= UniqueDropItemTableInput?.LoadDropItemTable();
            }
        }

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

        public virtual async UniTask DropItem(PlayerController player, Vector3 position)
        {
            var table = UniqueDropItemTable;
            if (table == null) table = Data.DefaultDropItemTable;

            position.y += 0.3f;

            await DropItem(player, position, table);
        }

        protected virtual async UniTask DropItem(PlayerController player, Vector3 position, DropItemData[] table)
        {
            var token = player.GetCancellationTokenOnDestroy();

            try
            {
                foreach (var item in table)
                {
                    token.ThrowIfCancellationRequested();

                    var rate = item.DropRate;
                    var random = UnityEngine.Random.Range(0f, 100f);
                    if (rate < random) continue;
                    DropItemSpawner.Instance.Spawn(position, item.Type, item.ID);

                    random = UnityEngine.Random.Range(0.1f, 0.3f);
                    await UniTask.Delay(TimeSpan.FromSeconds(random), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                // 処理がキャンセルされた場合の例外を無視
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}