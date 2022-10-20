using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス（OCEとはOnCollisionEnterの略）
/// 衝突時にヒット処理を行う弾基底クラス。
/// </summary>
public abstract class Base_BulletOCE : Base_Bullet
{
    #region Properties
    #endregion

    #region Inspector Variables
    #endregion

    #region Member Variables
    #endregion
    
    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision);
    }
    #endregion

    #region Virtual Methods
    protected virtual void OnHit(Collision collision)
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}