using Assets.Scripts.Dungeon.Areas;
using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Areas.Rooms;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        protected DungeonVisualizer dungeonVisualizer;
        protected EnemyGenerator enemyGenerator;

        [SerializeField]
        private GameObject playerCharacter;

        [SerializeField]
        private int minRoomCount = 8, maxRoomCount = 15;

        [SerializeField]
        [Range(9, 25)]
        private int roomWidth = 30, roomHeight = 20;

        [SerializeField]
        [Range(4, 6)]
        private int wallHeight = 4;

        [SerializeField]
        [Range(0, 10)]
        private int offset = 4;

        [SerializeField]
        [Range(4, 10)]
        private int corridorSize = 4;

        private void Start()
        {
            GenerateDungeon();
        }

        public void ClearDungeon()
        {
            // Cleanup objects
            enemyGenerator.DestroyEnemies();

            // Cleanup tilemap
            dungeonVisualizer.Clear();
        }

        public void GenerateDungeon()
        {
            dungeonVisualizer = GetComponent<DungeonVisualizer>();
            enemyGenerator = GetComponent<EnemyGenerator>();
            ClearDungeon();
            var rooms = CreateDungeon();

            playerCharacter.transform.position = (Vector3Int)rooms.GetRooms().First().RoomCenter;
        }

        private RoomManager CreateDungeon()
        {
            var roomVisualiser = new RoomVisualizer(dungeonVisualizer);
            var roomManager = new RoomManager(roomVisualiser, enemyGenerator);
            roomManager.SetWallHeight(wallHeight);
            roomManager.SetOffset(offset);
            roomManager.GenerateRooms(
                new RectInt(Vector2Int.zero, new Vector2Int(roomWidth, roomHeight)),
                minRoomCount,
                maxRoomCount
            );
            var corridorVisualiser = new CorridorVisualizer(dungeonVisualizer);
            var corridorManger = new CorridorManager(roomManager, corridorVisualiser);
            corridorManger.SetWallHeight(wallHeight);
            corridorManger.GenerateCorridors(corridorSize);

            dungeonVisualizer.SetAllFloorTiles();

            return roomManager;
        }
    }
}