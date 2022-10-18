using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾にアタッチすべきコンポーネントの基底クラス。<br/>
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

    #region Public Methods
    public override void OnDamage(
        EStatusManager enemyStatusManager,
        Rigidbody rigidbody = null, Vector3 dir = default, float knockbackPower = 1.0f)
    {
        // 相手のライフを減らす処理


        // ノックバック処理
        if (rigidbody == null)
        {
            Debug.LogError(
                "<color=grey>rigidbodyがnullです！修正してください！</color>");
        }
        else
        {
            rigidbody.AddForce(dir);
        }
    }
    #endregion

    #region Private Methods
    protected override void Init()
    {
        base.Init();
    }
    protected override void Process()
    {
        
    }
    #endregion

    #region On Animation Events
    #endregion
}