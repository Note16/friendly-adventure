using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Dungeon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Dungeon.Areas
{
    public class RoomManager
    {
        private readonly RoomVisualizer roomVisualizer;
        private List<Room> rooms;
        private int offset;

        public RoomManager(RoomVisualizer roomVisualizer)
        {
            this.roomVisualizer = roomVisualizer;
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
            while ((Random.value > 0.2f || roomsList.Count < minRoomCount) && roomsList.Count != maxRoomCount)
            {
                var newRoom = GenerateRoom(currentRoom.RoomRect, GetRandomDirection());
                if (!roomsList.Any(room => room.Position == newRoom.Position))
                {
                    currentRoom = newRoom;
                    roomsList.Add(newRoom);
                }
            }
            rooms = roomsList;
            AssignRoomTypes();
        }

        public IEnumerable<Room> GetRooms()
        {
            return rooms;
        }

        public Room GetAdjacentRoom(Room room, Direction direction)
        {
            return rooms.FirstOrDefault(r =>
            {
                return r.Position == GetRoomPosition(room.RoomRect, direction);
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
            return new Room(roomVisualizer, new RectInt(targetPosition, roomRect.size));
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

        private static Direction GetRandomDirection()
        {
            var values = Enum.GetValues(typeof(Direction));
            var random = new System.Random();
            return (Direction)values.GetValue(random.Next(values.Length));
        }
    }
}