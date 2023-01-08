using System;
using UniRx;
using UnityEngine;

namespace Human
{
    [System.Serializable]
    public class Talk
    {
        [Header("会話の内容")]
        [SerializeField]
        string[] _contents = { "未設定" };

        /// <summary> 話す内容のインデックス番号 </summary>
        private int _index = 0;
        /// <summary> 現在表示しているテキストの行を表す値 </summary>
        private int _currentLineNumber = 0;

        /// <summary> 会話アルゴリズムが完了したかどうかを表す値 </summary>
        public bool IsComplete { get; private set; } = false;

        /// <summary> 会話アルゴリズムの開始処理 </summary>
        public void StartTalk()
        {

        }
        /// <summary> 会話アルゴリズムの完了処理 </summary>
        /// <param name="onComplete"> 完了時に実行したいアクション </param>
        private void Complete(System.Action onComplete)
        {
            onComplete?.Invoke();

            _currentLineNumber = 0;
            _index++;
            _index %= _contents.Length;

            IsComplete = true;
            Observable.NextFrame()
                .Subscribe(_ => IsComplete = false);
        }
        /// <summary> 会話アルゴリズムの中断処理をここに記述する。 </summary>
        public void Exit()
        {
            _currentLineNumber = 0;
        }
    }
}