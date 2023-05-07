using Assets.Scripts.Dungeon.Visualizers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Corridors
{
    public class CorridorVertical
    {
        private readonly DungeonVisualizer dungeonVisualizer;
        public RectInt CorridorRect { get; }
        public RectInt OverlapRoomSouth { get; }
        public RectInt WallLeftRect { get; }
        public RectInt WallRightRect { get; }

        public CorridorVertical(DungeonVisualizer dungeonVisualizer, RectInt corridorRect, RectInt overlapRoomSouth)
        {
            this.dungeonVisualizer = dungeonVisualizer;
            CorridorRect = corridorRect;
            OverlapRoomSouth = overlapRoomSouth;
            WallLeftRect = new RectInt(CorridorRect.xMin, CorridorRect.yMin, 2, CorridorRect.height);
            WallRightRect = new RectInt(CorridorRect.xMax - 2, CorridorRect.yMin, 2, CorridorRect.height);


            ClearWalls();
            RenderFloor();
            RenderWalls();
        }

        private void ClearWalls()
        {
            var floor = CorridorRect.allPositionsWithin.ToVector2Int();
            dungeonVisualizer.ClearWallTiles(floor);
        }

        private void RenderFloor()
        {
            var roomFloor = new HashSet<Vector2Int>();
            foreach (var tile in CorridorRect.allPositionsWithin)
            {
                roomFloor.Add(tile);
            }

            dungeonVisualizer.SetFloorTiles(roomFloor, Color.grey);
        }

        private void RenderWalls()
        {
            dungeonVisualizer.SetCorridorWestWall(WallLeftRect, OverlapRoomSouth);
            dungeonVisualizer.SetCorridorEastWall(WallRightRect, OverlapRoomSouth);
        }
    }
}