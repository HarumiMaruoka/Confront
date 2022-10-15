using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの武器を制御するコンポーネント
/// </summary>
public class PWeaponComponent : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// 左クリックで実行する攻撃デリゲート（押した瞬間のみ実行するタイプ）
    /// </summary>
    public System.Action LeftClickAttackMoment { set => _leftClickAttackMoment = value; }
    /// <summary>
    /// 右クリックで実行する攻撃デリゲート（押した瞬間のみの実行するタイプ）
    /// </summary>
    public System.Action RightClickAttackMoment { set => _rightClickAttackMoment = value; }
    /// <summary>
    /// 左クリックで実行する攻撃デリゲート（押している間ずっと実行するタイプ）
    /// </summary>
    public System.Action LeftClickAttackNever { set => _leftClickAttackNever = value; }
    /// <summary>
    /// 右クリックで実行する攻撃デリゲート（押している間ずっと実行するタイプ）
    /// </summary>
    public System.Action RightClickAttackNever { set => _rightClickAttackNever = value; }
    #endregion

    #region Inspector Variables
    [Header("攻撃に関して設定すべき値")]
    [Tooltip("左手攻撃のボタン"), InputName, SerializeField]
    string _leftFireButtonName = "";
    [Tooltip("右手攻撃のボタン"), InputName, SerializeField]

    string _rightFireButtonName = "";
    #endregion

    #region Member Variables
    private System.Action _leftClickAttackMoment = default;
    private System.Action _rightClickAttackMoment = default;
    private System.Action _leftClickAttackNever = default;
    private System.Action _rightClickAttackNever = default;
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

    #region Private Methods
    private void UpdateWeapon()
    {

        if (Input.GetButtonDown(_leftFireButtonName))
        {
            _leftClickAttackMoment();
        }
        if (Input.GetButtonDown(_rightFireButtonName))
        {
            _rightClickAttackMoment();
        }

        if (Input.GetButton(_leftFireButtonName))
        {
            _leftClickAttackNever();
        }
        if (Input.GetButton(_rightFireButtonName))
        {
            _rightClickAttackNever();
        }
    }
    #endregion
}