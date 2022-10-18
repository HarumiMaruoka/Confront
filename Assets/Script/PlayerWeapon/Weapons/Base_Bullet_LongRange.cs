using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾にアタッチすべきコンポーネントの基底クラス。<br/>
/// 遠距離武器の弾
/// </summary>
public abstract class Base_Bullet_LongRange : Base_Bullet
{
    #region Properties
    #endregion

    #region Inspector Variables
    [Header("遠距離武器")]
    [Tooltip("弾が飛ぶ速度"), SerializeField]
    float _moveSpeed = 1f;
    #endregion

    #region Member Variables
    #endregion

    #region Public Methods
    public override void OnDamage(
        EStatusManager enemyStatusManager, Rigidbody _rigidbody = null, Vector3 dir = default, float knockbackPower = 1.0f)
    {

    }
    #endregion

    #region Private Methods
    protected override void Init()
    {
        base.Init();
    }
    protected virtual void Move()
    {

    }
    #endregion

    #region On Animation Events
    #endregion
}