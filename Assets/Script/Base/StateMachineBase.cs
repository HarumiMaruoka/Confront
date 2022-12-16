using System;
using UnityEngine;

[System.Serializable]
public abstract class StateMachineBase
{
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
    protected IState CurrentState { get; private set; }

    public event Action<IState> OnStateChanged = default;

    // 最初のステートを設定する。
    protected void Initialize(IState startState)
    {
        StateInit();

        CurrentState = startState;
        startState.Enter();

        // ステート変化時に実行するアクション。
        // 引数に最初のステートを渡す。
        OnStateChanged?.Invoke(startState);

#if UNITY_EDITOR
        OnStateChanged += 
            newState => 
            Debug.Log($"プレイヤーのステートが変更されました。\n" +
            $"現在のステートは\"{newState.GetType().Name}\"です。");
#endif
    }

    // ステートの遷移処理。引数に「次のステートの参照」を受け取る。
    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();      // 現在ステートの終了処理。
        CurrentState = nextState; // 現在のステートの変更処理。
        nextState.Enter();        // 変更された「新しい現在ステート」のEnter処理。

        // ステート変更時のアクションを実行する。
        // 引数に「新しい現在ステート」を渡す。
        OnStateChanged?.Invoke(nextState);
    }
    protected abstract void StateInit();
}