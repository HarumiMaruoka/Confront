using System;
using UnityEngine;

namespace Player
{
	[System.Serializable]
	public class EquipmentDataBase
	{
		public void LoadData(string addressableName)
        {
			// 各装備のパラメータをcsvファイルから読み込む。
			// （もしかしてセルのデータを数値にすればfloat.Parse()する必要ない？）
		}

		/// <summary> 装備の最大数 </summary>
		public const int MaxID = 0;

		private Equipment[] _equipmentData = new Equipment[MaxID];

		public Equipment[] EquipmentData => _equipmentData;
	}

	public class Equipment
    {
		// ステータス変化パラメータ

		// 装備名

		// 説明文
	}
}