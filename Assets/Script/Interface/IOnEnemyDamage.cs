using UnityEngine;
/// <summary>
/// 攻撃を与えるモノが実装すべきインターフェース
/// </summary>
public interface IOnEnemyDamage
{
    /// <summary> 攻撃処理 </summary>
    /// <param name="enemyStatusManager"> 
    /// 敵のステータスを管理しているコンポーネント
    /// </param>
    /// <param name="_rigidbody">
    /// ノックバックさせる場合に攻撃相手のRigidbodyをここに設定する。 
    /// </param>
    /// <param name="dir">
    /// ノックバックさせる場合にノックバック方向をここに設定する。
    /// </param>
    /// <param name="knockbackPower">
    /// ノックバックさせる場合にノックバックパワーをここに設定する。
    /// </param>
    void OnDamage(EnemyStatusBehavior enemyStatusManager, float value, bool isKnockback = false,
        Rigidbody _rigidbody = null, Vector3 dir = default, float knockbackPower = 1.0f);
}