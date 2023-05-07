using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorHorizontal
    {
        private readonly CorridorVisualizer corridorVisualizer;
        public RectInt Rect { get; }
        private RectInt wallTopRect { get; }
        private RectInt wallBottomRect { get; }

        public CorridorHorizontal(CorridorVisualizer corridorVisualizer, RectInt corridorRect)
        {
            this.corridorVisualizer = corridorVisualizer;
            Rect = corridorRect;

            var topWallHeight = 4;
            wallTopRect = new RectInt(Rect.xMin, Rect.yMax - topWallHeight / 2 + 1, Rect.width, topWallHeight);
            wallBottomRect = new RectInt(Rect.xMin, Rect.yMin, Rect.width, 1);


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
            corridorVisualizer.SetNorthWall(wallTopRect);
            corridorVisualizer.SetSouthWall(wallBottomRect);
        }
    }
}