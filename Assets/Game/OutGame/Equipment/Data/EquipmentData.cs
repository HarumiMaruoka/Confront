using System;
using UnityEngine;

namespace Confront.Equipment
{
    public class EquipmentData : ScriptableObject
    {
        public int Id;
        public string EquipmentName;
        public EquipmentType EquipmentType;
        public string Description;
        public Sprite Icon;

        [NonSerialized]
        public EquipmentStats[] StatsTable;
        [NonSerialized]
        public LevelRequirement[] LevelUpRequirements;

        public void Initialize()
        {
            StatsTable = Resources.Load<TextAsset>($"EquipmentTable{Id.ToString()}_Stats").LoadToStatsArray();
            LevelUpRequirements = Resources.Load<TextAsset>($"EquipmentTable{Id.ToString()}_UpgradeCost").LoadToLevelRequirementsArray();
        }

        public EquipmentStats GetStats(int level)
        {
            level--; // レベルは1から始まるため、0ベースに変換
            if (level < 0 || level >= StatsTable.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be within the range of the stats table.");
            }
            return StatsTable[level];
        }
    }
}