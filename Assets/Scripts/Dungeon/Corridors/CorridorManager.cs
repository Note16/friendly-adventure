using Assets.Scripts.Dungeon.Enums;
using Assets.Scripts.Dungeon.Rooms;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Corridors
{
    [RequireComponent(typeof(DungeonVisualizer))]
    public class CorridorManager : MonoBehaviour
    {
        private RoomManager roomManager;
        private CorridorVisualizer corridorVisualizer;
        private readonly int corridorSize = 6;

        public void Awake()
        {
            roomManager = GetComponent<RoomManager>();
            corridorVisualizer = new CorridorVisualizer(GetComponent<DungeonVisualizer>());
        }

        internal void GenerateCorridors()
        {
            var rooms = roomManager.GetRooms();

            foreach (var room in rooms)
            {
                var roomUp = roomManager.GetAdjacentRoom(room, Direction.Up);
                if (roomUp != null)
                {
                    var position = new Vector2Int(room.RectCenter.x - corridorSize / 2, room.Rect.yMax - room.Walls.Top.height);
                    var size = new Vector2Int(corridorSize, roomManager.corridorLength + room.Walls.Top.height + room.Walls.Bottom.height);

                    new CorridorVertical(corridorVisualizer, new RectInt(position, size), room.Walls.Top);
                }

                var roomRight = roomManager.GetAdjacentRoom(room, Direction.Right);
                if (roomRight != null)
                {
                    var position = new Vector2Int(room.Rect.xMax - room.Walls.Right.width, room.RectCenter.y - (corridorSize - 2) / 2 - room.Walls.Top.height / 2);
                    var size = new Vector2Int(roomManager.corridorLength + room.Walls.Left.width + room.Walls.Right.width, corridorSize - 2);

                    new CorridorHorizontal(corridorVisualizer, new RectInt(position, size));
                }
            }
        }
    }
}