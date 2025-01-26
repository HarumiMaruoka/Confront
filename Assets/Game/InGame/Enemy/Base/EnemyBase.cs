using Confront.DropItem;
using Confront.Player;
using Confront.SaveSystem;
using Confront.Utility;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, ISavable
    {
        public int ID;
        [Tooltip("このテーブルが設定されていない場合、EnemyDataのDefaultDropItemTableが使用される。")]
        public TextAsset UniqueDropItemTableInput;

        private string _saveKey = null;
        private string SaveKey => _saveKey ??= this.GetHierarchyPath(); // 同じ階層に同名のオブジェクトが存在しないことを前提としている。

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

        private void OnEnable()
        {
            var saveData = SaveDataController.Loaded;
            if (saveData != null && saveData.EnemyData.TryGetValue(SaveKey, out string data))
            {
                Load(data);
                saveData.EnemyData.Remove(SaveKey);
            }
        }

        protected virtual void Awake()
        {
            Data = EnemyManager.EnemySheet.GetData(ID);
            SavableRegistry.Register(this);
        }

        protected virtual void OnDestroy()
        {
            SavableRegistry.Unregister(this);
        }

        public static float DefaultCalculateDamage(float attackPower, float defense)
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
            DropItemData[] table = UniqueDropItemTable;
            if (table == null) table = Data.DefaultDropItemTable;
            if (table == null)
            {
                Debug.LogWarning("ドロップアイテムテーブルが設定されていません。");
                return;
            }

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
                    DropItemSpawner.Instance.Spawn(position, item.Type, item.ID, item.Amount);

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

        protected abstract string CreateSaveData();
        protected abstract void Load(string saveData);

        public void Save(SaveData saveData)
        {
            saveData.EnemyData[SaveKey] = CreateSaveData();
        }

        private void Load(SaveData saveData)
        {
            if (saveData.EnemyData.TryGetValue(SaveKey, out string data))
            {
                Load(data);
            }
        }
    }
}