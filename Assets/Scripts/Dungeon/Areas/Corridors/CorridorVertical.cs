using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorVertical
    {
        private readonly CorridorVisualizer corridorVisualizer;
        public RectInt Rect { get; }
        private RectInt overlapRoomSouth { get; }
        private RectInt wallLeftRect { get; }
        private RectInt wallRightRect { get; }

        public CorridorVertical(CorridorVisualizer corridorVisualizer, RectInt corridorRect, RectInt overlapRoomSouth)
        {
            this.corridorVisualizer = corridorVisualizer;
            this.Rect = corridorRect;
            this.overlapRoomSouth = overlapRoomSouth;
            wallLeftRect = new RectInt(Rect.xMin, Rect.yMin, 2, Rect.height);
            wallRightRect = new RectInt(Rect.xMax - 2, Rect.yMin, 2, Rect.height);

            ClearWalls();
            RenderFloor();
            RenderWalls();
        }

        private void ClearWalls()
        {
            var area = Rect.allPositionsWithin.ToVector2Int();
            corridorVisualizer.ClearWalls(area);
        }

        private void RenderFloor()
        {
            corridorVisualizer.SetFloor(Rect);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetWestWall(wallLeftRect, overlapRoomSouth);
            corridorVisualizer.SetEastWall(wallRightRect, overlapRoomSouth);
        }
    }
}