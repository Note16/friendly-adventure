using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class RoomFloor
    {
        public RectInt Floor { get; }
        public RectInt Inner { get; }
        public Vector2Int Center => Vector2Int.FloorToInt(Floor.center);

        public RoomFloor(RectInt rect, RoomWalls walls)
        {
            Floor = new RectInt(
                rect.xMin + walls.Left.width - 1,
                rect.yMin + walls.Bottom.height,
                rect.width - walls.Left.width - walls.Right.width + 2,
                rect.height - walls.Bottom.height - walls.Top.height + 1
            );

            Inner = new RectInt(
                rect.xMin + walls.Left.width,
                rect.yMin + walls.Bottom.height + 1,
                rect.width - walls.Left.width - walls.Right.width,
                rect.height - walls.Bottom.height - walls.Top.height - 1
            );
        }

        public void Render(RoomVisualizer roomVisualizer)
        {
            var area = Floor.allPositionsWithin.ToVector2Int();
            roomVisualizer.SetFloor(area);
        }
    }
}