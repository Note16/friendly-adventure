using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class RoomWalls
    {
        public RectInt Top { get; }
        public RectInt Right { get; }
        public RectInt Bottom { get; }
        public RectInt Left { get; }
        private int pillarDistance { get; set; } = 4;

        public RoomWalls(RectInt rect, int wallHeight, int pillarDistance)
        {
            Top = new RectInt(rect.xMin, rect.yMax - wallHeight, rect.width, wallHeight);
            Right = new RectInt(rect.xMax - 2, rect.yMin, 2, rect.height);
            Bottom = new RectInt(rect.xMin, rect.yMin, rect.width - 1, 1);
            Left = new RectInt(rect.xMin, rect.yMin, 2, rect.height);

            this.pillarDistance = pillarDistance;
        }

        public void Render(RoomVisualizer roomVisualizer)
        {
            roomVisualizer.SetNorthWall(Top, pillarDistance);
            roomVisualizer.SetEastWall(Right, Top, Bottom);
            roomVisualizer.SetSouthWall(Bottom);
            roomVisualizer.SetWestWall(Left, Top, Bottom);
        }

        public void CreateExit(RoomVisualizer roomVisualizer)
        {
            var exitRect = new RectInt(Top.xMax - 5, Top.yMin, 2, 2);
            roomVisualizer.ClearWallCollider(exitRect.allPositionsWithin.ToVector2Int());
        }
    }
}