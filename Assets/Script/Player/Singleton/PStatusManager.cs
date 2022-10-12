using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータスを管理するシングルトン
/// </summary>
public class PStatusManager
{
    #region Singleton
    private static PStatusManager _instance = new PStatusManager();
    public static PStatusManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }
    private PStatusManager(){ Init(); }
    #endregion

    #region Properties
    /// <summary>
    /// プレイヤーの基礎ステータス
    /// </summary>
    public PStatus BaseStatus => _baseStatus;
    /// <summary>
    /// レベル分のステータス上昇量
    /// </summary>
    public PStatus LevelStatus => _levelStatus;

    /// <summary>
    /// 装備分のステータス上昇量
    /// </summary>
    public PStatus EquipmentStatus => _equipmentStatus;
    /// <summary>
    /// アイテム分のステータス上昇量
    /// </summary>
    public PStatus ItemStatus => _itemStatus;
    /// <summary>
    /// 全てのステータス上昇量の合計値
    /// </summary>
    public PStatus TotalStatus { get => _baseStatus + _equipmentStatus + _itemStatus; }
    #endregion

    #region Member Variables
    private PStatus _baseStatus;
    private PStatus _levelStatus;
    private PStatus _equipmentStatus;
    private PStatus _itemStatus;
    private PStatus _totalStatus;
    #endregion

    #region Events
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Init()
    {
        // 基礎ステータスの初期化
        _baseStatus = new PStatus(10f, 10f, 10f, 10f, 10f, 10f);
    }
    #endregion
}