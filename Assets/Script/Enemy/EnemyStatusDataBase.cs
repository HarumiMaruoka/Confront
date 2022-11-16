using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusDataBase
{
    #region Singleton
    private static EnemyStatusDataBase _instance = new EnemyStatusDataBase();
    public static EnemyStatusDataBase Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }
    private EnemyStatusDataBase() { }
    #endregion

    private EnemyStatus[] _enemyStatuses = default;

    /// <summary>
    /// 各エネミーのステータス : <br/>
    /// インデックスにIDを指定することでステータスにアクセスすることができる。
    /// </summary>
    public EnemyStatus[] EnemyStatuses 
    {
        get 
        {
            if (_enemyStatuses == null)
            {
                Debug.LogError("EnemyStatusesがNullです！");
            }
            return _enemyStatuses; 
        }
    }

    /// <summary>
    /// 敵のステータスを設定する。
    /// </summary>
    /// <param name="statuses"></param>
    public void SetEnemyStatuses(EnemyStatus[] statuses)
    {
        _enemyStatuses = statuses;
    }
}