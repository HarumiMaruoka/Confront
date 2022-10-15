using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス。<br/>
/// 近距離武器の弾。
/// </summary>
public abstract class Base_Bullet_ShortRange : Base_Bullet
{
    #region Properties
    #endregion

    #region Inspector Variables
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

    }
    #endregion

    #region Public Methods
    public override void Damage(ref float life)
    {
        life -= PStatusManager.Instance.TotalStatus._shortRangeOffensivePower;
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
    #endregion
}