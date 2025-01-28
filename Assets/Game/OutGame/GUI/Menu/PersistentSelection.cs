using UnityEngine;
using UnityEngine.EventSystems;

namespace Confront.GameUI
{
    // 何もないところをクリックしたときに選択状態が解除されるのでそれを防ぐ。
    public class PersistentSelection : MonoBehaviour
    {
        private GameObject lastSelectedObject;  // 最後に選択されたオブジェクト

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                // 現在選択されているオブジェクトを記憶
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
            else if (
                EventSystem.current.currentSelectedGameObject == null &&
                lastSelectedObject != null)
            {
                // 選択状態が解除された場合、最後のオブジェクトを再選択
                EventSystem.current.SetSelectedGameObject(lastSelectedObject);
            }
        }
    }
}