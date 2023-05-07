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
            wallLeftRect = new RectInt(this.Rect.xMin, this.Rect.yMin, 2, this.Rect.height);
            wallRightRect = new RectInt(this.Rect.xMax - 2, this.Rect.yMin, 2, this.Rect.height);

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
            corridorVisualizer.SetFloor(Rect, Color.grey);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetWestWall(wallLeftRect, overlapRoomSouth);
            corridorVisualizer.SetEastWall(wallRightRect, overlapRoomSouth);
        }
    }
}