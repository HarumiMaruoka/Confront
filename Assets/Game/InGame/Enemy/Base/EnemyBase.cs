using Confront.DropItem;
using Confront.Enemy.Slimey;
using Confront.Player;
using Confront.SaveSystem;
using Confront.Utility;
using Cysharp.Threading.Tasks;
using NexEditor;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, ISavable
    {
        [Header("Common")]
        public int ID;
        [Expandable]
        public EnemyStats Stats;
        [Tooltip("このテーブルが設定されていない場合、EnemyDataのDefaultDropItemTableが使用される。")]
        public TextAsset UniqueDropItemTableInput;
        public LifeGauge LifeGauge;

        private string _saveKey = null;
        private Vector3 _initialPosition;
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

        protected virtual void Awake()
        {
            _initialPosition = transform.position;
            Data = EnemyManager.EnemySheet.GetData(ID);
            EnemyManager.OnEnemiesReset += Reset;

            Stats = Instantiate(Stats);
            Stats.Health = Stats.MaxHealth;

            if (LifeGauge)
            {
                LifeGauge.gameObject.SetActive(EnemyManager.ShowEnemyLifeGauge);
                EnemyManager.OnShowEnemyLifeGaugeChanged += OnLifeGaugeVisibilityChanged;

                LifeGauge.Initialize(Stats.MaxHealth);
                LifeGauge.UpdateHealth(Stats.Health);
                Stats.OnLifeChanged += LifeGauge.UpdateHealth;
            }

            SavableRegistry.Register(this);
        }

        protected virtual void OnDestroy()
        {
            EnemyManager.OnEnemiesReset -= Reset;

            if (LifeGauge)
            {
                EnemyManager.OnShowEnemyLifeGaugeChanged -= OnLifeGaugeVisibilityChanged;
                Stats.OnLifeChanged -= LifeGauge.UpdateHealth;
            }

            SavableRegistry.Unregister(this);
        }

        protected virtual void Reset()
        {
            Stats.Health = Stats.MaxHealth;
            transform.position = _initialPosition;
            gameObject.SetActive(true);
        }

        protected virtual void OnEnable()
        {
            var saveData = SaveDataController.Loaded;
            if (saveData != null && saveData.EnemyData.TryGetValue(SaveKey, out string data))
            {
                Load(data);
                saveData.EnemyData.Remove(SaveKey);
            }
        }

        // ダメージを受けるときに、実行するダメージ計算処理。
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

        public virtual async UniTask DropItem(Vector3 position)
        {
            DropItemData[] table = UniqueDropItemTable;
            if (table == null) table = Data.DefaultDropItemTable;
            if (table == null)
            {
                Debug.LogWarning("ドロップアイテムテーブルが設定されていません。");
                return;
            }

            position.y += 0.3f;

            await DropItem(position, table);
        }

        protected virtual async UniTask DropItem(Vector3 position, DropItemData[] table)
        {
            var token = PlayerController.Instance.GetCancellationTokenOnDestroy();

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

        private void OnLifeGaugeVisibilityChanged(bool isVisible)
        {
            if (this && LifeGauge) LifeGauge.gameObject.SetActive(isVisible);
            else
            {
                EnemyManager.OnShowEnemyLifeGaugeChanged -= OnLifeGaugeVisibilityChanged;
                Stats.OnLifeChanged -= LifeGauge.UpdateHealth;
            }
        }
    }
}