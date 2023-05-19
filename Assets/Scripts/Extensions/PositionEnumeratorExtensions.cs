using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RectInt;

namespace Assets.Scripts.Extensions
{
    public static class PositionEnumeratorExtensions
    {
        public static List<Vector2Int> ToVector2Int(this PositionEnumerator positionEnumerator)
        {
            var vector2IntList = new List<Vector2Int>();
            foreach (var item in positionEnumerator)
            {
                vector2IntList.Add(item);
            }
            return vector2IntList;
        }
    }
}