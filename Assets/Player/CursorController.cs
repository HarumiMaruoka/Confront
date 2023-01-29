using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [InputName, SerializeField]
    private string _cancelButton = default;

    private bool _previousIsLock = false;

    void Update()
    {
        if (Input.GetButtonDown(_cancelButton))
        {
            if (_previousIsLock)
            {
                _previousIsLock = false;
                //カーソルのロック解除
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                _previousIsLock = true;
                // カーソルを画面中央にロックする
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
