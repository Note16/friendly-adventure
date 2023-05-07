using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorVertical
    {
        private readonly CorridorVisualizer corridorVisualizer;
        private RectInt corridorRect { get; }
        private RectInt overlapRoomSouth { get; }
        private RectInt wallLeftRect { get; }
        private RectInt wallRightRect { get; }

        public CorridorVertical(CorridorVisualizer corridorVisualizer, RectInt corridorRect, RectInt overlapRoomSouth)
        {
            this.corridorVisualizer = corridorVisualizer;
            this.corridorRect = corridorRect;
            this.overlapRoomSouth = overlapRoomSouth;
            wallLeftRect = new RectInt(this.corridorRect.xMin, this.corridorRect.yMin, 2, this.corridorRect.height);
            wallRightRect = new RectInt(this.corridorRect.xMax - 2, this.corridorRect.yMin, 2, this.corridorRect.height);

            ClearWalls();
            RenderFloor();
            RenderWalls();
        }

        private void ClearWalls()
        {
            var area = corridorRect.allPositionsWithin.ToVector2Int();
            corridorVisualizer.ClearWalls(area);
        }

        private void RenderFloor()
        {
            var area = corridorRect.allPositionsWithin.ToVector2Int();
            corridorVisualizer.SetFloor(area, Color.grey);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetCorridorWestWall(wallLeftRect, overlapRoomSouth);
            corridorVisualizer.SetCorridorEastWall(wallRightRect, overlapRoomSouth);
        }
    }
}