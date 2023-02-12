using UnityEngine;

/// <summary>
/// Enemyのステータスを管理するクラス
/// </summary>
public class EnemyStatusController : MonoBehaviour, ITakenDamage
{
    [SerializeField]
    private EnemyStatus _status = default;

    public EnemyStatus Status => _status;

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void TakenDamage(float value)
    {
        // ライフを減らす処理
        _status._life -= value;
        // 死判定
        if (_status._life <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// 死亡時の処理
    /// </summary>
    private void Death()
    {
        Destroy(gameObject);
    }
}
