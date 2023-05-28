using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorHorizontal
    {
        private readonly CorridorVisualizer corridorVisualizer;
        public RectInt Rect { get; }
        private RectInt wallTopRect { get; }
        private RectInt wallBottomRect { get; }

        public CorridorHorizontal(CorridorVisualizer corridorVisualizer, RectInt corridorRect, int wallHeight)
        {
            this.corridorVisualizer = corridorVisualizer;
            Rect = corridorRect;

            wallTopRect = new RectInt(Rect.xMin, Rect.yMax - wallHeight / 2 + 1, Rect.width, wallHeight);
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
            corridorVisualizer.SetFloor(Rect);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetNorthWall(wallTopRect);
            corridorVisualizer.SetSouthWall(wallBottomRect);
        }
    }
}