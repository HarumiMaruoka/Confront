using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Title
{
    public class StartButton : MonoBehaviour
    {
        // 最後にセーブされたデータを読み込む
        private void Start() => GetComponent<Button>().onClick.AddListener(() => Debug.Log("Not Implemented"));
    }
}