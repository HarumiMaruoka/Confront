using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Confront.Enemy
{
    public static class EnemyManager
    {
        static EnemyManager()
        {
            var handle = Addressables.LoadAssetAsync<EnemySheet>("EnemySheet");
            handle.WaitForCompletion();
            EnemySheet = handle.Result;
            EnemySheet.Initialize();
        }

        public static EnemySheet EnemySheet { get; }
        public static Action OnEnemiesReset;

        private static bool _showEnemyLifeGauge = false;
        public static event Action<bool> OnShowEnemyLifeGaugeChanged;
        public static bool ShowEnemyLifeGauge
        {
            get => _showEnemyLifeGauge;
            set
            {
                if (_showEnemyLifeGauge == value) return;
                _showEnemyLifeGauge = value;
                OnShowEnemyLifeGaugeChanged?.Invoke(value);
            }
        }
    }
}