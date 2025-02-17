using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.NotificationManager
{
    public class NotificationView : MonoBehaviour
    {
        public RectTransform RectTransform;

        private Notification _notification;
        private Vector2 _targetPosition;
        private float _elapsed;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private TMPro.TextMeshProUGUI _title;
        [SerializeField]
        private TMPro.TextMeshProUGUI _message;
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private AnimationCurve _fadeCurve;
        [SerializeField]
        private float _moveSpeed = 10f;

        [NonSerialized] public RectTransform Container;

        public event Action<NotificationView> OnNotificationEnd;

        public Notification Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                _title.text = _notification.Title;
                _message.text = _notification.Message;
                _icon.sprite = _notification.Icon;
                Initialize();
            }
        }

        public Vector2 TargetPosition
        {
            get => _targetPosition;
            set
            {
                _targetPosition = value;
                MoveUp().Forget();
            }
        }

        private void Initialize()
        {
            _elapsed = 0f;

            var initialX = Container.rect.width / 2f - RectTransform.rect.width / 2f;
            var initialY = -Container.rect.height / 2f - RectTransform.rect.height / 2f;
            var initialPos = new Vector2(initialX, initialY);

            RectTransform.anchoredPosition = initialPos;

            TargetPosition = new Vector2(initialX, initialY);
        }

        private void OnDisable()
        {
            _moveCTS?.Cancel();
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            _canvasGroup.alpha = _fadeCurve.Evaluate(_elapsed);

            if (_elapsed >= _fadeCurve.keys[_fadeCurve.length - 1].time)
            {
                OnNotificationEnd?.Invoke(this);
            }
        }

        private CancellationTokenSource _moveCTS;

        private async UniTask MoveUp()
        {
            _moveCTS?.Cancel();
            _moveCTS = new CancellationTokenSource();
            var token = _moveCTS.Token;

            while (!token.IsCancellationRequested && RectTransform.anchoredPosition.y < _targetPosition.y && this)
            {
                RectTransform.anchoredPosition += Vector2.up * _moveSpeed * Time.deltaTime;
                await UniTask.Yield();
            }

            if (!token.IsCancellationRequested && this)
            {
                RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, _targetPosition.y);
            }
        }
    }
}