using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Confront.GUI
{
    // メニュー画面の説明ウィンドウに情報を送信するクラス。
    public class MenuDescriptionSender : MonoBehaviour, ISelectHandler
    {
        public static event Action<Sprite, Sprite, string, string> OnDescriptionUpdate;

        [SerializeField, Tooltip("生成時に表示するかどうか")]
        private bool _sendOnStart = false;
        [SerializeField]
        private Sprite _background;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private string _title;
        [SerializeField]
        private string _description;

        private void Start()
        {
            if (_sendOnStart) SendDescription();
        }

        private void OnEnable()
        {
            if (_sendOnStart) SendDescription();
        }

        private void SendDescription()
        {
            OnDescriptionUpdate?.Invoke(_background, _icon, _title, _description);
        }

        public void OnSelect(BaseEventData eventData)
        {
            SendDescription();
        }
    }
}