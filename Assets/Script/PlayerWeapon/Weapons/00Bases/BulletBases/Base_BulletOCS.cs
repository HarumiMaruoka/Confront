using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス（OCSとはOnCollisionStyaの略）
/// 衝突中ずっとヒット処理を行うクラス。
/// </summary>
public abstract class Base_BulletOCS : Base_Bullet
{
    #region Properties
    #endregion

    #region Inspector Variables
    #endregion

    #region Member Variables
    #endregion
    
    #region Unity Methods
    private void OnCollisionStay(Collision collision)
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