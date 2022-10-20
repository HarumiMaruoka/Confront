using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス（OTSとはOnTriggerStayの略）
/// 衝突時にヒット処理を行う弾基底クラス。
/// </summary>
public abstract class Base_BulletOTS : Base_Bullet
{
    #region Properties
    #endregion

    #region Inspector Variables
    #endregion

    #region Member Variables
    #endregion

    #region Unity Methods
    private void OnTriggerStay(Collider other)
    {
        OnHit(other);
    }

    #endregion

    #region Virtual Methods
    protected virtual void OnHit(Collider other)
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}