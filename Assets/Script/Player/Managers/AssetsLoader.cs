using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセットの読み込みを制御するクラス。
/// </summary>
public class AssetsLoader
{
    #region Singleton
    private static AssetsLoader _instance = new AssetsLoader();
    public static AssetsLoader Instance
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
    private AssetsLoader(){}
    #endregion

    #region Public Methods
    public void OnLoadAssets()
    {
        // データを読み込み各データベースに保存する。
    }
    #endregion
}