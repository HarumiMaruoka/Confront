using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備のステータス上昇量を管理するシングルトン
/// </summary>
public class EquipmentStatusManager
{
    #region Singleton
    private static EquipmentStatusManager _instance = new EquipmentStatusManager();
    public static EquipmentStatusManager Instance
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
    private EquipmentStatusManager(){}
    #endregion

    #region Properties
    /// <summary>
    /// 装備分のステータス上昇量
    /// </summary>
    public PStatus EquipmentStatus => _equipmentStatus;
    #endregion

    #region Member Variables
    private PStatus _equipmentStatus;
    #endregion

    #region Events
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}