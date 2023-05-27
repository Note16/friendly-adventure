using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Enums;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas
{
    public class CorridorManager
    {
        private readonly RoomManager roomManager;
        private readonly CorridorVisualizer corridorVisualizer;
        private int wallHeight = 4;

        public CorridorManager(RoomManager roomManager, CorridorVisualizer corridorVisualizer)
        {
            this.roomManager = roomManager;
            this.corridorVisualizer = corridorVisualizer;
        }

        public void SetWallHeight(int wallHeight)
        {
            this.wallHeight = wallHeight;
        }

        internal void GenerateCorridors(int size)
        {
            var rooms = roomManager.GetRooms();

            foreach (var room in rooms)
            {
                var roomUp = roomManager.GetAdjacentRoom(room, Direction.Up);
                if (roomUp != null)
                {
                    var position = new Vector2Int(room.RectCenter.x - size / 2, room.Rect.yMax - room.Walls.Top.height);
                    var corridor = new Vector2Int(size, roomManager.GetOffset() + room.Walls.Top.height + room.Walls.Bottom.height);

                    new CorridorVertical(corridorVisualizer, new RectInt(position, corridor), room.Walls.Top);
                }

                var roomRight = roomManager.GetAdjacentRoom(room, Direction.Right);
                if (roomRight != null)
                {
                    var position = new Vector2Int(room.Rect.xMax - room.Walls.Right.width, room.RectCenter.y - (size - 2) / 2 - room.Walls.Top.height / 2);
                    var corridor = new Vector2Int(roomManager.GetOffset() + room.Walls.Left.width + room.Walls.Right.width, size - 2);

                    new CorridorHorizontal(corridorVisualizer, new RectInt(position, corridor), wallHeight);
                }
            }
        }
    }
}