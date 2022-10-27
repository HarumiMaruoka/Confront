/// <summary>
/// 敵のステータスを管理するための構造体
/// </summary>
[System.Serializable]
public struct EnemyStatus
{
    public float _life;

    EnemyStatus(float life)
    {
        _life = life;
    }
}