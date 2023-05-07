using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Enums;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas
{
    public class CorridorManager
    {
        private readonly RoomManager roomManager;
        private readonly CorridorVisualizer corridorVisualizer;

        public CorridorManager(RoomManager roomManager, CorridorVisualizer corridorVisualizer)
        {
            this.roomManager = roomManager;
            this.corridorVisualizer = corridorVisualizer;
        }

        internal void GenerateCorridors(int size)
        {
            var rooms = roomManager.GetRooms();

            foreach (var room in rooms)
            {
                var roomUp = roomManager.GetAdjacentRoom(room, Direction.Up);
                if (roomUp != null)
                {
                    var position = new Vector2Int(room.RoomCenter.x - size / 2, room.Rect.yMax - room.WallTopRect.height);
                    var corridor = new Vector2Int(size, roomManager.GetOffset() + room.WallTopRect.height + room.WallBottomRect.height);

                    new CorridorVertical(corridorVisualizer, new RectInt(position, corridor), room.WallTopRect);
                }

                var roomRight = roomManager.GetAdjacentRoom(room, Direction.Right);
                if (roomRight != null)
                {
                    var position = new Vector2Int(room.Rect.xMax - room.WallRightRect.width, room.RoomCenter.y - (size - 2) / 2 - room.WallTopRect.height / 2);
                    var corridor = new Vector2Int(roomManager.GetOffset() + room.WallLeftRect.width + room.WallRightRect.width, size - 2);

                    new CorridorHorizontal(corridorVisualizer, new RectInt(position, corridor));
                }
            }
        }
    }
}