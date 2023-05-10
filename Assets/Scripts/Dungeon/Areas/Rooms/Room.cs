using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class Room
    {
        private readonly RoomVisualizer roomVisualizer;
        private RoomType roomType { get; set; }
        private Color roomColor { get; set; }
        public RectInt Rect { get; }
        public RectInt WallTopRect { get; }
        public RectInt WallBottomRect { get; }
        public RectInt WallLeftRect { get; }
        public RectInt WallRightRect { get; }
        public Vector2Int RoomCenter => Vector2Int.FloorToInt(Rect.center);
        private int pillarDistance { get; set; } = 4;
        private int wallHeight { get; set; } = 4;

        public Room(RoomVisualizer roomVisualizer, RectInt roomRect)
        {
            this.roomVisualizer = roomVisualizer;
            Rect = roomRect;

            WallTopRect = new RectInt(Rect.xMin, Rect.yMax - wallHeight, Rect.width, wallHeight);
            WallBottomRect = new RectInt(Rect.xMin, Rect.yMin, Rect.width, 1);
            WallLeftRect = new RectInt(Rect.xMin, Rect.yMin, 2, Rect.height);
            WallRightRect = new RectInt(Rect.xMax - 2, Rect.yMin, 2, Rect.height);

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
            var area = Rect.allPositionsWithin.ToVector2Int();
            roomVisualizer.SetFloor(area, roomColor);
        }

        private void RenderWalls()
        {
            roomVisualizer.SetNorthWall(WallTopRect, pillarDistance);
            roomVisualizer.SetSouthWall(WallBottomRect);
            roomVisualizer.SetWestWall(WallLeftRect, WallTopRect, WallBottomRect);
            roomVisualizer.SetEastWall(WallRightRect, WallTopRect, WallBottomRect);
        }
    }
}