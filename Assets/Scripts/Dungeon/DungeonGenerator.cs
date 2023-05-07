using Assets.Scripts.Dungeon.Areas;
using Assets.Scripts.Dungeon.Visualizers;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        protected DungeonVisualizer dungeonVisualizer;

        [SerializeField]
        protected Vector2Int startPosition = Vector2Int.zero;
        [SerializeField]
        private int roomWidth = 30, roomHeight = 20, minRoomCount = 8, maxRoomCount = 15;

        [SerializeField]
        [Range(0, 10)]
        private int offset = 4;

        public void GenerateDungeon()
        {
            dungeonVisualizer = GetComponent<DungeonVisualizer>();
            dungeonVisualizer.Clear();
            CreateRooms();
        }

        private void CreateRooms()
        {
            var roomManager = new RoomManager(dungeonVisualizer);
            roomManager.SetOffset(offset);
            roomManager.GenerateRooms(
                new RectInt(startPosition, new Vector2Int(roomWidth, roomHeight)),
                minRoomCount,
                maxRoomCount
            );
            var corridorManger = new CorridorManager(roomManager, dungeonVisualizer);

            //var corridors = corridorManger.GetCorridors();
            corridorManger.GenerateCorridors();

            //var room = rooms.First();
            //tilemapVisualizer.PaintFloorTiles(new List<Vector2Int> { room.RoomCenter }, Color.white);
            //var closestRoom = corridorManger.GetClosestRoom(room);
            //tilemapVisualizer.PaintFloorTiles(new List<Vector2Int> { closestRoom.RoomCenter }, Color.white);

            // Create floor
            //dungeonVisualizer.PaintFloorTiles(corridors, Color.white);

            // Create walls
            //WallGenerator.CreateWalls(floor, tilemapVisualizer);
        }
    }
}