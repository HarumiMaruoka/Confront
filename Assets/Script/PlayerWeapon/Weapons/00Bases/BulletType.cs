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
	/// <summary> 近距離攻撃 / 飛ばないもの / 生まれた場所から動かないもの </summary>
	NOT_MOVE,
	/// <summary> その他 / 召喚モノとか？ </summary>
	SPECIAL,
}

public enum HitType
{
	/// <summary> 未設定 </summary>
	NOT_SET,
	/// <summary> コライダー衝突時のみ </summary>
	ON_COLLISION_ENTER,
	/// <summary> コライダー衝突中ずっと </summary>
	ON_COLLISION_STAY,
	/// <summary> トリガー接触時のみ </summary>
	ON_TRIGGER_ENTER,
	/// <summary> トリガー接触中ずっと </summary>
	ON_TRIGGER_STAY,
	/// <summary> レイ接触 </summary>
	RAY_HIT_,

	/// <summary> その他 </summary>
	ATHER,
}