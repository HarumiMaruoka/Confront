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
    private string _enemyTagName = "";
    #endregion

    #region Member Variables
    // Components
    private Animator _animator;

    // Athor Variables
    protected readonly BulletType _bulletType = BulletType.NOT_SET;
    protected readonly HitType _hitType = HitType.NOT_SET;
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
    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    private void Process()
    {
        switch (_bulletType)
        {
            case BulletType.NOT_SET:
                Debug.LogError("<color=grey>_bulletTypeが未設定です！修正してください！</color>");
                break;
            case BulletType.MOVE:
                Move();
                break;
            case BulletType.NOT_MOVE:
                break;
            case BulletType.SPECIAL:
                Special();
                break;
            default:
                Debug.LogError("<color=grey>_bulletTypeに不正な値が入っています！修正してください！</color>");
                break;
        }
    }
    #endregion

    #region Virtual Methods
    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    protected virtual void Move() { Debug.LogWarning("<color=grey>継承し中身を実装してください！</color>"); }
    /// <summary>
    /// 特殊武器処理
    /// </summary>
    protected virtual void Special() { Debug.LogWarning("<color=grey>継承し中身を実装してください！</color>"); }
    /// <summary>
    /// 攻撃処理 <br/>
    /// オーバーライド可 <br/>
    /// </summary>
    /// <param name="enemyStatusManager"> 攻撃相手のステータス管理クラス </param>
    /// <param name="value"> 減らすライフの量 </param>
    /// <param name="isKnockback"> ノックバックするかどうか </param>
    /// <param name="_rigidbody"> ノックバックさせる場合に攻撃相手のRigidbodyをここに設定する。</param>
    /// <param name="dir"> ノックバックさせる場合にノックバック方向をここに設定する。</param>
    /// <param name="knockbackPower"> ノックバックの強さ </param>
    public virtual void OnDamage(
        EStatusManager enemyStatusManager, float value, bool isKnockback = false,
        Rigidbody _rigidbody = null, Vector3 dir = default, float knockbackPower = 1.0f)
    {
        // 体力を減らす処理
        var temp = enemyStatusManager.Status;
        temp._life -= value;
        enemyStatusManager.Status = temp;

        // ノックバック処理
        if (isKnockback)
        {
            if (_rigidbody == null)
            {
                Debug.LogError(
                    $"<color=grey>_rigidbodyがnullです！修正してください！</color>");
            }
            else
            {
                _rigidbody.AddForce(dir.normalized * knockbackPower, ForceMode.Impulse);
            }
        }
    }
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