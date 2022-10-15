using UnityEngine;

/// <summary>
/// 弾の種類を表すenum
/// </summary>
[HideInInspector]
public enum BulletType
{
	/// <summary> 未設定 </summary>
	NOT_SET,
	/// <summary> 飛ぶもの / 動くもの </summary>
	MOVE,
	/// <summary> 近距離攻撃 / 飛ばないもの </summary>
	NOT_MOVE,
	/// <summary> その他 </summary>
	ATHER,
}