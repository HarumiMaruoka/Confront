using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の基底クラス。
/// </summary>
public abstract class Base_Bullet : MonoBehaviour, IOnDamage
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
    protected virtual void Update()
    {

    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    protected virtual void Init()
    {

    }
    public abstract void Damage(ref float life);
    #endregion

    #region On Animation Events
    private void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion
}