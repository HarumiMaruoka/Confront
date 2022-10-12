using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーにアタッチすべきコンポーネント : <br/>
/// 接地判定と接地対象の情報を提供する。 <br/>
/// </summary>
public class PIsGround : MonoBehaviour
{
    #region Properties
    /// <summary> 接地しているかどうかを表すプロパティ </summary>
    public bool IsGround => _isGround;
    #endregion

    #region Inspector Variables
    [Header("接地判定に使用する値")]
    [Tooltip("レイの最大距離"), SerializeField]
    private float _maxDistance = default;
    [Tooltip("円の半径"), SerializeField]
    private float _radius = 1.0f;
    [Tooltip("接地判定を行う対象(LayerMask)"), SerializeField]
    private LayerMask _layerMask = default;
#if UNITY_EDITOR
    [Header("デバッグ関連"),]
    [Tooltip("レイをデバッグ表示するかどうか"), SerializeField]
    private bool _isGizmo = false;
#endif
    #endregion
    #region Member Variables
    private bool _isGround = default;
    #endregion

    #region Events
    #endregion

    #region Unity Methods

    private void Start()
    {

    }

    private void Update()
    {
        GetIsGround();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_isGizmo)
        {
            Gizmos.DrawWireSphere(transform.position + Vector3.down * _maxDistance, _radius);
        }
    }
#endif

    #endregion

    #region Public Methods
    /// <summary>
    /// 接地判定の更新 <br/>
    /// 戻り値に接触対象の情報を返す。
    /// </summary>
    /// <returns>
    /// 接触対象の情報
    /// </returns>
    public RaycastHit GetIsGround()
    {
        _isGround = Physics.SphereCast(transform.position, _radius, Vector3.down, out RaycastHit hit, _maxDistance, _layerMask);
        return hit;
    }
    #endregion

    #region Private Methods
    #endregion
}