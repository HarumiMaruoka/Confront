using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.NotificationManager
{
    public class Notifier : MonoBehaviour
    {
        private static event Action<Notification> OnNotificationAdded;
        public static void AddNotification(string title, string message, Sprite icon)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Icon = icon
            };
            OnNotificationAdded?.Invoke(notification);
        }

        [SerializeField]
        private NotificationView _notificationPrefab;
        [SerializeField]
        private RectTransform _container;

        private HashSet<NotificationView> _actives = new HashSet<NotificationView>();
        private Stack<NotificationView> _inactives = new Stack<NotificationView>();


        private void Start()
        {
            OnNotificationAdded += OnAdded;
        }

        private void OnDestroy()
        {
            OnNotificationAdded -= OnAdded;
        }

        private void UpdateTargetPosition()
        {
            foreach (var elem in _actives)
            {
                var targetPosition = elem.TargetPosition;
                targetPosition.y += elem.RectTransform.rect.height;
                elem.TargetPosition = targetPosition;
            }
        }

        private void OnAdded(Notification notification)
        {
            var element = GetOrCreateElement();
            element.Notification = notification;
            element.OnNotificationEnd -= OnNotificationEnd; // 重複登録を防ぐ
            element.OnNotificationEnd += OnNotificationEnd;
            UpdateTargetPosition();
        }

        private NotificationView GetOrCreateElement()
        {
            NotificationView elem;

            if (_inactives.Count > 0)
            {
                elem = _inactives.Pop();
                elem.gameObject.SetActive(true);
                elem.transform.SetAsLastSibling();
            }
            else
            {
                elem = Instantiate(_notificationPrefab, _container);
            }

            _actives.Add(elem);
            return elem;
        }

        private void OnNotificationEnd(NotificationView element)
        {
            element.gameObject.SetActive(false);
            _inactives.Push(element);
            _actives.Remove(element);
        }
    }

    public struct Notification
    {
        public string Title;
        public string Message;
        public Sprite Icon;
    }
}