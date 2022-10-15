/// <summary>
/// プレイヤーのステータスを表す構造体
/// </summary>
[System.Serializable]
public struct PStatus
{
    /// <summary> 体力 </summary>
    public float _life;
    /// <summary> スタミナ </summary>
    public float _stamina;
    /// <summary> 移動速度 </summary>
    public float _moveSpeed;
    /// <summary> 近距離武器攻撃力 </summary>
    public float _shortRangeOffensivePower;
    /// <summary> 遠距離武器攻撃力 </summary>
    public float _longRangeOffensivePower;
    /// <summary> 重さ </summary>
    public float _weight;

    public PStatus(float life = 0.0f, float stamina = 0.0f, float moveSpeed = 0.0f,
        float leftOffensivePower = 0.0f, float rightOffensivePower = 0.0f, float weight = 0.0f)
    {
        _life = life;
        _stamina = stamina;
        _moveSpeed = moveSpeed;
        _shortRangeOffensivePower = leftOffensivePower;
        _longRangeOffensivePower = rightOffensivePower;
        _weight = weight;
    }

    public static PStatus operator +(PStatus left, PStatus right)
    {
        left._life += right._life;
        left._stamina += right._stamina;
        left._moveSpeed += right._moveSpeed;
        left._shortRangeOffensivePower += right._shortRangeOffensivePower;
        left._longRangeOffensivePower += right._longRangeOffensivePower;
        left._weight += right._weight;
        return left;
    }
}