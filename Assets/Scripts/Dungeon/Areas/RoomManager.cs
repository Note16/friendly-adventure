using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Dungeon.Enums;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas
{
    public class RoomManager
    {
        private readonly RoomVisualizer roomVisualizer;
        private readonly EnemyGenerator enemyGenerator;
        private List<Room> rooms;
        private int offset;
        private int wallHeight = 4;
        private int pillarDistance = 4;

        public RoomManager(RoomVisualizer roomVisualizer, EnemyGenerator enemyGenerator)
        {
            this.roomVisualizer = roomVisualizer;
            this.enemyGenerator = enemyGenerator;
        }

        public void GenerateEnemies()
        {
            foreach (var room in rooms.Where(room => room.GetRoomType() == RoomType.Default))
            {
                enemyGenerator.GenerateEnemies(room, 4);
            }

            var bossRoom = rooms.First(room => room.GetRoomType() == RoomType.BossRoom);
            enemyGenerator.GenerateBossEnemy(bossRoom);
        }

        public void SetWallHeight(int wallHeight)
        {
            this.wallHeight = wallHeight;
        }

        public void SetOffset(int offset)
        {
            this.offset = offset;
        }

        public int GetOffset()
        {
            return offset;
        }

        public void GenerateRooms(RectInt roomRect, int minRoomCount, int maxRoomCount)
        {
            var currentRoom = GenerateRoom(roomRect, null);
            var roomsList = new List<Room>
            {
                currentRoom
            };
            while ((RandomHelper.GetRandom(70) || roomsList.Count < minRoomCount) && roomsList.Count != maxRoomCount)
            {
                var newRoom = GenerateRoom(currentRoom.Rect, RandomHelper.GetRandom<Direction>());
                if (!roomsList.Any(room => room.Rect.position == newRoom.Rect.position))
                {
                    roomsList.Add(newRoom);
                }

                currentRoom = newRoom;
            }
            rooms = roomsList;
            AssignRoomTypes();
            GenerateEnemies();
        }

        public IEnumerable<Room> GetRooms()
        {
            return rooms;
        }

        public Room GetAdjacentRoom(Room room, Direction direction)
        {
            return rooms.FirstOrDefault(r =>
            {
                return r.Rect.position == GetRoomPosition(room.Rect, direction);
            });
        }

        private void AssignRoomTypes()
        {
            rooms.First().SetRoomType(RoomType.Entrance);
            rooms.Last().SetRoomType(RoomType.BossRoom);
        }

        private Room GenerateRoom(RectInt roomRect, Direction? direction)
        {
            var targetPosition = GetRoomPosition(roomRect, direction);
            return new Room(roomVisualizer, new RectInt(targetPosition, roomRect.size), wallHeight, pillarDistance);
        }

        private Vector2Int GetRoomPosition(RectInt roomRect, Direction? direction)
        {
            var targetPosition = roomRect.position;
            if (direction == Direction.Up)
                targetPosition += new Vector2Int(0, roomRect.height + offset);
            if (direction == Direction.Down)
                targetPosition += new Vector2Int(0, -(roomRect.height + offset));
            if (direction == Direction.Left)
                targetPosition += new Vector2Int(-(roomRect.width + offset), 0);
            if (direction == Direction.Right)
                targetPosition += new Vector2Int(roomRect.width + offset, 0);

            return targetPosition;
        }
    }
}