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
        private GameObject entranceRoom;

        [SerializeField]
        private GameObject defaultRoom;

        [SerializeField]
        private GameObject shopRoom;

        [SerializeField]
        private GameObject bossRoom;

        private RoomVisualizer roomVisualizer;
        private List<Room> rooms;

        private readonly int roomHeight = 18;
        private readonly int roomWidth = 18;
        public readonly int corridorSize = 4;

        public void Awake()
        {
            roomVisualizer = new RoomVisualizer(GetComponent<DungeonVisualizer>());
        }

        public void GenerateRooms(int minRoomCount, int maxRoomCount)
        {
            var roomRects = GenerateRoomLayout(minRoomCount, maxRoomCount);
            var rooms = AssignRoomTypes(roomRects).ToList();
            rooms.ForEach(r => r.GenerateObjects());
            this.rooms = rooms;
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

        private List<RectInt> GenerateRoomLayout(int minRoomCount, int maxRoomCount)
        {
            var room = new RectInt(Vector2Int.zero, new Vector2Int(roomWidth, roomHeight));
            var roomRects = new List<RectInt>
            {
                room
            };

            while ((RandomHelper.GetRandom(95) || roomRects.Count < minRoomCount) && roomRects.Count != maxRoomCount)
            {
                var attemptedDirection = new List<Direction>();

                Vector2Int? targetPosition = roomRects.Last().position;
                while (roomRects.Any(roomRect => roomRect.position == targetPosition))
                {
                    if (attemptedDirection.Count >= Enum.GetNames(typeof(Direction)).Length)
                        break;

                    var randomDirection = RandomHelper.GetRandomEnum(attemptedDirection);
                    attemptedDirection.Add(randomDirection);
                    targetPosition = GetRoomPosition(roomRects.Last(), randomDirection);
                }

                if (targetPosition == null)
                    break;

                roomRects.Add(new RectInt(targetPosition.Value, room.size));
            }

            return roomRects;
        }

        private IEnumerable<Room> AssignRoomTypes(List<RectInt> roomLayout)
        {
            for (int i = 0; i < roomLayout.Count; i++)
            {
                bool isFirstRoom = i == 0;
                if (isFirstRoom)
                {
                    yield return GenerateRoom(roomLayout[i], entranceRoom);
                    continue;
                }

                bool isSecondLastRoom = i == roomLayout.Count - 2;
                if (isSecondLastRoom)
                {
                    yield return GenerateRoom(roomLayout[i], shopRoom);
                    continue;
                }

                bool isLastRoom = i == roomLayout.Count - 1;
                if (isLastRoom)
                {
                    yield return GenerateRoom(roomLayout[i], bossRoom);
                    continue;
                }

                yield return GenerateRoom(roomLayout[i], defaultRoom);
            }
        }

        private Room GenerateRoom(RectInt roomRect, GameObject roomObject)
        {
            var room = Instantiate(roomObject, new Vector3(roomRect.position.x, roomRect.position.y, 0), Quaternion.identity, gameObject.transform).GetComponent<Room>();
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