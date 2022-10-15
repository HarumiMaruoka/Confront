/// <summary>
/// 敵のステータスを管理するための構造体
/// </summary>
[System.Serializable]
public struct EStatus
{
    public float _life;

    EStatus(float life)
    {
        _life = life;
    }
}