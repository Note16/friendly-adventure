using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class Room
    {
        private readonly RoomVisualizer roomVisualizer;
        private RoomType roomType { get; set; }
        private Color roomColor { get; set; }
        public RectInt RoomRect { get; }
        public RectInt WallTopRect { get; }
        public RectInt WallBottomRect { get; }
        public RectInt WallLeftRect { get; }
        public RectInt WallRightRect { get; }
        public Vector2Int Position => RoomRect.position;
        public Vector2Int RoomCenter => Vector2Int.FloorToInt(RoomRect.center);

        public Room(RoomVisualizer roomVisualizer, RectInt roomRect)
        {
            this.roomVisualizer = roomVisualizer;
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
            var area = RoomRect.allPositionsWithin.ToVector2Int();
            roomVisualizer.SetFloor(area, roomColor);
        }

        private void RenderWalls()
        {
            roomVisualizer.SetRoomNorthWall(WallTopRect);
            roomVisualizer.SetRoomSouthWall(WallBottomRect);
            roomVisualizer.SetRoomWestWall(WallLeftRect, WallTopRect, WallBottomRect);
            roomVisualizer.SetRoomEastWall(WallRightRect, WallTopRect, WallBottomRect);
        }
    }
}