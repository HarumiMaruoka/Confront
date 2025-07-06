using System;
using System.Collections.Generic;
using Confront.ForgeItem;
using Confront.Player;

namespace Confront.Equipment
{
    /// <summary>
    /// 装備のレベルアップ処理を管理するクラス。
    /// テスト容易性を考慮して依存性はコンストラクタで注入されます。
    /// </summary>
    public sealed class EquipmentLevelUp
    {
        private readonly CurrencyManager _currency;
        private readonly ForgeItemInventory _inventory;
        private readonly LevelRequirement[] _requirements;

        /// <summary>現在のレベル（0ベース）</summary>
        public int Level { get; private set; }

        public EquipmentLevelUp(CurrencyManager currency,
                                ForgeItemInventory inventory,
                                LevelRequirement[] requirements,
                                int initialLevel = 0)
        {
            _currency = currency ?? throw new ArgumentNullException(nameof(currency));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _requirements = requirements ?? throw new ArgumentNullException(nameof(requirements));
            Level = initialLevel;
        }

        /// <summary>
        /// レベルアップ可能かどうかを確認し、不足している資源の情報を返す。
        /// </summary>
        public bool CanLevelUp(out Shortage shortage)
        {
            if (Level >= _requirements.Length)
            {
                shortage = Shortage.MaxLevelReached;
                return false;
            }

            var req = _requirements[Level];

            int missingFunds = Math.Max(0, req.Price - _currency.Balance);

            var missingMats = new MaterialRequirement[req.Materials.Length];

            for (int i = 0; i < req.Materials.Length; i++)
            {
                var mat = req.Materials[i];
                int have = _inventory.GetCount(mat.ItemId);
                int missing = Math.Max(0, mat.Amount - have); // 不足分を計算、不足ナシなら0。
                missingMats[i] = new MaterialRequirement(mat.ItemId, missing);
            }

            shortage = new Shortage(missingFunds, missingMats);
            return shortage.IsSatisfied;
        }

        /// <summary>
        /// レベルアップ条件を満たしている場合に、資源を消費してレベルを1上げる。
        /// </summary>
        public void LevelUp()
        {
            if (!CanLevelUp(out var shortage))
                throw new InvalidOperationException($"Cannot level up: {shortage}");

            var req = _requirements[Level];

            _currency.Debit(req.Price);
            foreach (var mat in req.Materials)
                _inventory.Remove(mat.ItemId, mat.Amount);

            Level++;
        }
    }

    #region Value Objects

    /// <summary>必要素材と数量を表す構造体。</summary>
    public readonly struct MaterialRequirement
    {
        public int ItemId { get; }
        public int Amount { get; }

        public MaterialRequirement(int itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public override string ToString() => $"(ID: {ItemId}, Amount: {Amount})";
    }

    /// <summary>レベルアップ1回分に必要なコストを定義する構造体。</summary>
    public readonly struct LevelRequirement
    {
        public int Price { get; }
        public MaterialRequirement[] Materials { get; }

        public LevelRequirement(int price, MaterialRequirement[] materials)
        {
            Price = price;
            Materials = materials;
        }
    }

    /// <summary>レベルアップに不足している資源情報をまとめる構造体。</summary>
    public readonly struct Shortage
    {
        public static readonly Shortage None = new Shortage(0, Array.Empty<MaterialRequirement>());
        public static readonly Shortage MaxLevelReached = new Shortage(-1, Array.Empty<MaterialRequirement>());

        /// <summary>不足している通貨量。</summary>
        public int MissingFunds { get; }
        /// <summary>不足している素材の一覧。</summary>
        public MaterialRequirement[] MissingMaterials { get; }

        /// <summary>レベルアップ条件をすべて満たしているかどうか。</summary>
        public bool IsSatisfied => MissingFunds == 0 && MissingMaterials.Length == 0;

        public Shortage(int missingFunds, MaterialRequirement[] missingMaterials)
        {
            MissingFunds = missingFunds;
            MissingMaterials = missingMaterials;
        }

        public override string ToString() =>
            IsSatisfied ? "No shortage" :
            $"MissingFunds: {MissingFunds}, MissingMaterials: [{string.Join(", ", MissingMaterials)}]";
    }

    #endregion
}
