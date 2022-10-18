using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾にアタッチすべきコンポーネントの基底クラス。<br/>
/// </summary>
public abstract class Base_Bullet : MonoBehaviour, IOnEnemyDamage
{
    #region Properties
    #endregion

    #region Inspector Variables
    [TagName, SerializeField]
    string _enemyTagName = "";
    #endregion

    #region Member Variables
    Animator _animator;
    #endregion

    #region Constant
    #endregion

    #region Unity Methods
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Process();
    }
    /// <summary> 接触時処理 </summary>
    /// <param name="other"> 接触相手のコライダー </param>
    private void OnTriggerEnter(Collider other)
    {

    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
    }
    protected abstract void Process();
    /// <summary>
    /// <para>
    /// 攻撃処理 <br/>
    /// 派生先,実装先で独自の攻撃処理を記述してください！
    /// </para>
    /// 想定する処理  : <br/>
    /// 近距離攻撃ではノックバックし、<br/>
    /// 遠距離攻撃ではノックバックしない。<br/>
    /// 独自の挙動をするものでは、<br/>
    /// 例えば、遠距離攻撃でノックバックするものでは <br/>
    /// オーバーライドして処理を記述してください。
    /// </summary>
    /// <param name="life"> 攻撃相手の体力 </param>
    /// <param name="_rigidbody"> ノックバックさせる場合に攻撃相手のRigidbodyをここに設定する。</param>
    /// <param name="dir"> ノックバックさせる場合にノックバック方向をここに設定する。</param>
    public abstract void OnDamage(
        EStatusManager enemyStatusManager,
        Rigidbody _rigidbody = null, Vector3 dir = default, float knockbackPower = 1.0f);
    #endregion

    #region On Animation Events
    /// <summary>
    /// このゲームオブジェクトを破棄する。<br/>
    /// アニメーションイベントから呼び出す想定で作成したメソッド。
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion
}