using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class ScrollToSelected : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect; // ScrollRectの参照
        [SerializeField]
        private RectTransform content; // ScrollRectのContentの参照

        private bool warningFlag = false;

        private void Update()
        {
            if ((!MenuController.Instance || !MenuController.Instance.CurrentMenu))
            {
                if (!warningFlag)
                {
                    warningFlag = true;
                    Debug.LogWarning("MenuController or CurrentMenu is null");
                }
                return;
            }

            if (MenuController.Instance.CurrentMenu == this.gameObject)
            {
                var selected = UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject?.GetComponent<RectTransform>();
                if (selected) ScrollTo(selected);
            }
        }

        /// <summary>
        /// 指定された要素をScrollViewの範囲内に収める
        /// </summary>
        /// <param name="target">対象のRectTransform</param>
        public void ScrollTo(RectTransform target)
        {
            // ScrollRectのViewportの参照
            RectTransform viewport = scrollRect.viewport;

            // Content内の対象要素の座標
            Vector3[] contentCorners = new Vector3[4];
            content.GetWorldCorners(contentCorners);

            Vector3[] targetCorners = new Vector3[4];
            target.GetWorldCorners(targetCorners);

            // Viewportの座標
            Vector3[] viewportCorners = new Vector3[4];
            viewport.GetWorldCorners(viewportCorners);

            // 対象の上下位置を計算
            float contentHeight = contentCorners[2].y - contentCorners[0].y;
            float viewportHeight = viewportCorners[2].y - viewportCorners[0].y;

            float targetMinY = targetCorners[0].y;
            float targetMaxY = targetCorners[2].y;

            // Contentの相対座標
            float contentMinY = contentCorners[0].y;
            float contentMaxY = contentCorners[2].y;

            // ScrollView内の現在のスクロール位置
            Vector2 normalizedPosition = scrollRect.normalizedPosition;

            // 対象が上に見切れている場合
            if (targetMaxY > viewportCorners[2].y)
            {
                float delta = targetMaxY - viewportCorners[2].y;
                normalizedPosition.y += delta / contentHeight;
            }
            // 対象が下に見切れている場合
            else if (targetMinY < viewportCorners[0].y)
            {
                float delta = viewportCorners[0].y - targetMinY;
                normalizedPosition.y -= delta / contentHeight;
            }

            // スクロール位置を調整
            scrollRect.normalizedPosition = normalizedPosition;
        }
    }
}