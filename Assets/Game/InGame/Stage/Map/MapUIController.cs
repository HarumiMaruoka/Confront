using System;
using UnityEngine;

namespace Confront.Stage.Map
{
    public class MapUIController : MonoBehaviour
    {
        [SerializeField]
        private Grid _grid;

        public void A(Vector2 mapPosition)
        {
            var cellPosition = _grid.WorldToCell(new Vector3(mapPosition.x, mapPosition.y, 0));

        }
    }
}