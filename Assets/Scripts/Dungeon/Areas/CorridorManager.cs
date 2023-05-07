using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Enums;
using Assets.Scripts.Dungeon.Visualizers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Areas
{
    public class CorridorManager
    {
        private List<CorridorVertical> corridors;
        private readonly RoomManager roomManager;
        private readonly DungeonVisualizer dungeonVisualizer;

        public CorridorManager(RoomManager roomManager, DungeonVisualizer dungeonVisualizer)
        {
            this.roomManager = roomManager;
            this.dungeonVisualizer = dungeonVisualizer;
        }

        internal void GenerateCorridors()
        {
            var rooms = roomManager.GetRooms();
            var corridorSize = 5;

            var corridors = new List<CorridorVertical>();
            foreach (var room in rooms)
            {
                var roomUp = roomManager.GetAdjacentRoom(room, Direction.Up);
                if (roomUp != null)
                {
                    var position = new Vector2Int(room.RoomCenter.x - corridorSize / 2, room.RoomRect.yMax - room.WallTopRect.height);
                    var corridor = new Vector2Int(corridorSize, roomManager.GetOffset() + room.WallTopRect.height + room.WallBottomRect.height);

                    corridors.Add(new CorridorVertical(dungeonVisualizer, new RectInt(position, corridor), room.WallTopRect));
                }

                var roomRight = roomManager.GetAdjacentRoom(room, Direction.Right);
                if (roomRight != null)
                {
                    var position = new Vector2Int(room.RoomRect.xMax - room.WallRightRect.width, room.RoomCenter.y - corridorSize / 2);
                    var corridor = new Vector2Int(roomManager.GetOffset() + room.WallLeftRect.width + room.WallRightRect.width, corridorSize);

                    //corridors.Add(new Corridor(dungeonVisualizer, new RectInt(position, corridor)));
                }
            }
            this.corridors = corridors;
        }
    }
}