using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorHorizontal
    {
        private readonly CorridorVisualizer corridorVisualizer;
        public RectInt CorridorRect { get; }
        private RectInt overlapRoomSouth { get; }
        private RectInt wallLeftRect { get; }
        private RectInt wallRightRect { get; }

        public CorridorHorizontal(CorridorVisualizer corridorVisualizer, RectInt corridorRect, RectInt overlapRoomSouth)
        {
            this.corridorVisualizer = corridorVisualizer;
            CorridorRect = corridorRect;
            this.overlapRoomSouth = overlapRoomSouth;
            wallLeftRect = new RectInt(CorridorRect.xMin, CorridorRect.yMin, 2, CorridorRect.height);
            wallRightRect = new RectInt(CorridorRect.xMax - 2, CorridorRect.yMin, 2, CorridorRect.height);


            ClearWalls();
            RenderFloor();
            RenderWalls();
        }

        private void ClearWalls()
        {
            var floor = CorridorRect.allPositionsWithin.ToVector2Int();
            //corridorVisualizer.ClearWallTiles(floor);
        }

        private void RenderFloor()
        {
            var roomFloor = new HashSet<Vector2Int>();
            foreach (var tile in CorridorRect.allPositionsWithin)
            {
                roomFloor.Add(tile);
            }
            //corridorVisualizer.SetFloorTiles(roomFloor, Color.grey);
        }

        private void RenderWalls()
        {
            corridorVisualizer.SetCorridorWestWall(wallLeftRect, overlapRoomSouth);
            corridorVisualizer.SetCorridorEastWall(wallRightRect, overlapRoomSouth);
        }
    }
}