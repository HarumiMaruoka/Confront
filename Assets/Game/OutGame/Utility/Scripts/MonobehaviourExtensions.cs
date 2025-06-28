using System;
using UnityEngine;

namespace Confront.Utility
{
    public static class MonobehaviourExtensions
    {
        public static string GetHierarchyPath(this MonoBehaviour monobehaviour)
        {
            // Transformから開始
            Transform current = monobehaviour.transform;

            // パスの各要素を格納するリスト
            System.Collections.Generic.List<string> pathElements = new System.Collections.Generic.List<string>();

            // ルートオブジェクトに到達するまで親をたどる
            while (current != null)
            {
                pathElements.Add(current.name);
                current = current.parent;
            }

            // ルート側からの順序に直して、"/"で結合
            pathElements.Reverse();
            return string.Join("/", pathElements);
        }
    }
}