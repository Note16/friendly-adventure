using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas.Rooms
{
    public class Room
    {
        private RoomType roomType { get; set; }
        public RoomWalls Walls { get; }
        public RoomFloor Floor { get; }
        public RectInt Rect { get; }
        public Vector2Int RectCenter => Vector2Int.FloorToInt(Rect.center);

        public Room(RoomVisualizer roomVisualizer, RectInt roomRect, int wallHeight, int pillarDistance)
        {
            Rect = roomRect;
            Walls = new RoomWalls(Rect, wallHeight, pillarDistance);
            Floor = new RoomFloor(Rect, Walls);

            Floor.Render(roomVisualizer);
            Walls.Render(roomVisualizer);
        }

        public void SetRoomType(RoomType type) => roomType = type;
        public RoomType GetRoomType() => roomType;
    }
}