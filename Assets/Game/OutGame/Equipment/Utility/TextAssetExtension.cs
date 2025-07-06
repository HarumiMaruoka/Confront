using NUnit.Framework;
using UnityEngine;

namespace Confront.Equipment
{
    public static class TextAssetExtension
    {
        public static EquipmentStats[] LoadToStatsArray(this TextAsset csvAsset)
        {
            var lines = csvAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var stats = new EquipmentStats[lines.Length - 1]; // 最初の行はヘッダーなので除外
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                if (values.Length < 8)
                {
                    Debug.LogError($"CSV行 {i + 1} の値が不足しています: {lines[i]}");
                    continue;
                }
                try
                {
                    stats[i - 1] = new EquipmentStats(
                        int.Parse(values[0]),
                        int.Parse(values[1]),
                        int.Parse(values[2]),
                        int.Parse(values[3]),
                        int.Parse(values[4]),
                        int.Parse(values[5]),
                        int.Parse(values[6]),
                        int.Parse(values[7])
                    );
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"CSV行 {i + 1} の解析中にエラーが発生しました: {e.Message}");
                    continue;
                }
            }
            return stats;
        }

        public static LevelRequirement[] LoadToLevelRequirementsArray(this TextAsset csvAsset)
        {
            var lines = csvAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            var requirements = new LevelRequirement[lines.Length - 1]; // 最初の行はヘッダーなので除外
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                if (values.Length < 3)
                {
                    Debug.LogError($"CSV行 {i + 1} の値が不足しています: {lines[i]}");
                    continue;
                }
                try
                {
                    int price = int.Parse(values[0]);
                    var materials = new MaterialRequirement[values.Length - 1];
                    for (int j = 1; j < values.Length; j++)
                    {
                        var matValues = values[j].Split(':');
                        if (matValues.Length != 2)
                        {
                            Debug.LogError($"CSV行 {i + 1} の材料情報が不正です: {values[j]}");
                            continue;
                        }
                        materials[j - 1] = new MaterialRequirement(int.Parse(matValues[0]), int.Parse(matValues[1]));
                    }
                    requirements[i - 1] = new LevelRequirement(price, materials);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"CSV行 {i + 1} の解析中にエラーが発生しました: {e.Message}");
                    continue;
                }
            }
            return requirements;
        }
    }
}