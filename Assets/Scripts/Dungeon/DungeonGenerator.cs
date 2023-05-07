using Assets.Scripts.Dungeon.Areas;
using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Areas.Rooms;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        protected DungeonVisualizer dungeonVisualizer;

        [SerializeField]
        private int minRoomCount = 8, maxRoomCount = 15;

        [SerializeField]
        [Range(9, 100)]
        private int roomWidth = 30, roomHeight = 20;

        [SerializeField]
        [Range(0, 10)]
        private int offset = 4;

        [SerializeField]
        [Range(4, 10)]
        private int corridorSize = 4;

        public void GenerateDungeon()
        {
            dungeonVisualizer = GetComponent<DungeonVisualizer>();
            dungeonVisualizer.Clear();
            CreateRooms();
        }

        private void CreateRooms()
        {
            var roomVisualiser = new RoomVisualizer(dungeonVisualizer);
            var roomManager = new RoomManager(roomVisualiser);
            roomManager.SetOffset(offset);
            roomManager.GenerateRooms(
                new RectInt(Vector2Int.zero, new Vector2Int(roomWidth, roomHeight)),
                minRoomCount,
                maxRoomCount
            );
            var corridorVisualiser = new CorridorVisualizer(dungeonVisualizer);
            var corridorManger = new CorridorManager(roomManager, corridorVisualiser);
            corridorManger.GenerateCorridors(corridorSize);
        }
    }
}