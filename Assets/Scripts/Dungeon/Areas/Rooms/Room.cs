using Assets.Scripts.Dungeon.Visualizers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class Room
    {
        private readonly DungeonVisualizer dungeonVisualizer;
        private RoomType roomType { get; set; }
        private Color roomColor { get; set; }
        public RectInt RoomRect { get; }
        public RectInt WallTopRect { get; }
        public RectInt WallBottomRect { get; }
        public RectInt WallLeftRect { get; }
        public RectInt WallRightRect { get; }
        public Vector2Int Position => RoomRect.position;
        public Vector2Int RoomCenter => Vector2Int.FloorToInt(RoomRect.center);

        public Room(DungeonVisualizer dungeonVisualizer, RectInt roomRect)
        {
            this.dungeonVisualizer = dungeonVisualizer;
            RoomRect = roomRect;
            WallTopRect = new RectInt(RoomRect.xMin, RoomRect.yMax - 4, RoomRect.width, 4);
            WallBottomRect = new RectInt(RoomRect.xMin, RoomRect.yMin, RoomRect.width, 1);
            WallLeftRect = new RectInt(RoomRect.xMin, RoomRect.yMin, 2, RoomRect.height);
            WallRightRect = new RectInt(RoomRect.xMax - 2, RoomRect.yMin, 2, RoomRect.height);

            SetRoomType(RoomType.Default);
            RenderWalls();
        }

        public void SetRoomType(RoomType roomType)
        {
            this.roomType = roomType;
            SetRoomColor();
            RenderFloor();
        }

        private void SetRoomColor()
        {
            roomColor = roomType switch
            {
                RoomType.Entrance => Color.cyan,
                RoomType.Shop => Color.yellow,
                RoomType.BossRoom => Color.red,
                _ => Color.gray,
            };
        }

        private void RenderFloor()
        {
            var roomFloor = new HashSet<Vector2Int>();
            foreach (var tile in RoomRect.allPositionsWithin)
            {
                roomFloor.Add(tile);
            }

            dungeonVisualizer.SetFloorTiles(roomFloor, roomColor);
        }

        private void RenderWalls()
        {
            dungeonVisualizer.SetRoomNorthWall(WallTopRect);
            dungeonVisualizer.SetRoomSouthWall(WallBottomRect);
            dungeonVisualizer.SetRoomWestWall(WallLeftRect, WallTopRect, WallBottomRect);
            dungeonVisualizer.SetRoomEastWall(WallRightRect, WallTopRect, WallBottomRect);
        }
    }
}