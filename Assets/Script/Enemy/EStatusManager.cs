using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するコンポーネント。
/// </summary>
public class EStatusManager : MonoBehaviour
{
    #region Properties
    public EStatus Status { get => _status; set => _status = value; }
    #endregion

    #region Inspector Variables
    [Header("エネミーに設定すべき値")]
    [Tooltip(""), SerializeField]
    private EStatus _status = default;
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

    }

    private void Update()
    {

    }

    #endregion

    #region Enums
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}