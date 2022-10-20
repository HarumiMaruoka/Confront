using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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