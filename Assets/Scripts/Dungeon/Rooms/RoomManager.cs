using Assets.Scripts.Dungeon.Enums;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Rooms
{
    [RequireComponent(typeof(DungeonVisualizer))]
    public class RoomManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject defaultRoom;

        private RoomVisualizer roomVisualizer;
        private List<Room> rooms;

        private readonly int roomSize = 18;
        private readonly int roomWidth = 18;
        public readonly int corridorSize = 4;

        public void Awake()
        {
            roomVisualizer = new RoomVisualizer(GetComponent<DungeonVisualizer>());
        }

        public void GenerateRooms(int minRoomCount, int maxRoomCount)
        {
            var room = GenerateRoom(new RectInt(Vector2Int.zero, new Vector2Int(roomSize, roomWidth)));
            var roomsList = new List<Room>
            {
                room
            };
            while ((RandomHelper.GetRandom(95) || roomsList.Count < minRoomCount) && roomsList.Count != maxRoomCount)
            {
                var attemptedDirection = new List<Direction>();

                Vector2Int? targetPosition = null;
                while (roomsList.Any(room => targetPosition == null || room.Rect.position == targetPosition))
                {
                    if (attemptedDirection.Count >= Enum.GetNames(typeof(Direction)).Length)
                        break;

                    var randomDirection = RandomHelper.GetRandomEnum(attemptedDirection);
                    attemptedDirection.Add(randomDirection);
                    targetPosition = GetRoomPosition(room.Rect, randomDirection);
                }

                if (targetPosition == null)
                    break;

                room = GenerateRoom(new RectInt(targetPosition.Value, room.Rect.size));
                roomsList.Add(room);
            }
            rooms = roomsList;
            AssignRoomTypes();
            rooms.ForEach(r => r.GenerateObjects());
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

            var defaultRooms = rooms.Where(rooms => rooms.RoomType == RoomType.Default).ToList();
            var randomRoom = RandomHelper.GetRandom(defaultRooms);
            randomRoom.SetRoomType(RoomType.Shop);
        }

        private Room GenerateRoom(RectInt roomRect)
        {
            var room = Instantiate(defaultRoom, new Vector3(roomRect.position.x, roomRect.position.y, 0), Quaternion.identity, gameObject.transform).GetComponent<Room>();
            room.Awake();
            room.SetRoomVisualizer(roomVisualizer);
            room.Rect = new RectInt(roomRect.position, roomRect.size);
            room.Create();
            return room;
        }

        private Vector2Int GetRoomPosition(RectInt roomRect, Direction direction)
        {
            var targetPosition = roomRect.position;
            if (direction == Direction.Up)
                targetPosition += new Vector2Int(0, roomRect.height + corridorSize);
            if (direction == Direction.Down)
                targetPosition += new Vector2Int(0, -(roomRect.height + corridorSize));
            if (direction == Direction.Left)
                targetPosition += new Vector2Int(-(roomRect.width + corridorSize), 0);
            if (direction == Direction.Right)
                targetPosition += new Vector2Int(roomRect.width + corridorSize, 0);

            return targetPosition;
        }
    }
}