using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Corridors
{
    public class CorridorVertical
    {
        private CorridorVisualizer corridorVisualizer;
        private RectInt rect;
        private RectInt overlapRoomSouth;
        private RectInt wallLeftRect;
        private RectInt wallRightRect;

        public CorridorVertical(CorridorVisualizer corridorVisualizer, RectInt rect, RectInt overlapRoomSouth)
        {
            this.corridorVisualizer = corridorVisualizer;
            this.rect = rect;
            this.overlapRoomSouth = overlapRoomSouth;
            wallLeftRect = new RectInt(rect.xMin, rect.yMin, 2, rect.height);
            wallRightRect = new RectInt(rect.xMax - 2, rect.yMin, 2, rect.height);

            ClearWalls();
            RenderFloor();
            RenderWalls();
        }

        private void ClearWalls()
        {
            var area = rect.allPositionsWithin.ToVector2Int();
            corridorVisualizer.ClearWalls(area);
        }

        private void RenderFloor()
        {
            corridorVisualizer.SetFloor(rect);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetWestWall(wallLeftRect, overlapRoomSouth);
            corridorVisualizer.SetEastWall(wallRightRect, overlapRoomSouth);
        }
    }
}