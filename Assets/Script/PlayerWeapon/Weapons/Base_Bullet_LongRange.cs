using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス。<br/>
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

    #region Constant
    #endregion

    #region Events
    #endregion

    #region Unity Methods
    private void Start()
    {
        Init();
    }
    protected override void Update()
    {
        Move();
    }
    #endregion

    #region Public Methods
    public override void Damage(ref float life)
    {
        life -= PStatusManager.Instance.TotalStatus._longRangeOffensivePower;
    }
    #endregion

    #region Private Methods
    protected override void Init()
    {

    }
    protected virtual void Move()
    {

    }
    #endregion

    #region On Animation Events
    private void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

}