using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Corridors
{
    public class CorridorHorizontal
    {
        private CorridorVisualizer corridorVisualizer;
        private RectInt rect;
        private RectInt wallTopRect;
        private RectInt wallBottomRect;

        private int wallHeight = 5;

        public CorridorHorizontal(CorridorVisualizer corridorVisualizer, RectInt rect)
        {
            this.corridorVisualizer = corridorVisualizer;
            this.rect = rect;
            wallTopRect = new RectInt(rect.xMin, rect.yMax - wallHeight / 2 + 1, rect.width, wallHeight);
            wallBottomRect = new RectInt(rect.xMin, rect.yMin, rect.width, 1);

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
            corridorVisualizer.SetNorthWall(wallTopRect);
            corridorVisualizer.SetSouthWall(wallBottomRect);
        }
    }
}