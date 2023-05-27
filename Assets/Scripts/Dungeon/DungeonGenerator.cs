using Assets.Scripts.Dungeon.Areas;
using Assets.Scripts.Dungeon.Areas.Corridors;
using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        protected DungeonVisualizer dungeonVisualizer;
        protected EnemyGenerator enemyGenerator;
        protected ObjectsGenerator objectsGenerator;

        [SerializeField]
        private GameObject playerCharacter;

        [SerializeField]
        private int minRoomCount = 8, maxRoomCount = 15;

        [SerializeField]
        [Range(18, 25)]
        private int roomWidth = 18, roomHeight = 18;

        [SerializeField]
        [Range(4, 6)]
        private int wallHeight = 4;

        [SerializeField]
        [Range(0, 10)]
        private int offset = 4;

        [SerializeField]
        [Range(6, 10)]
        private int corridorSize = 4;

        [SerializeField]
        private bool disableGenerateOnStart = false;

        private void Start()
        {
            if (!disableGenerateOnStart)
                GenerateDungeon();
        }

        private void InitComponents()
        {
            if (dungeonVisualizer == null) dungeonVisualizer = GetComponent<DungeonVisualizer>();
            if (enemyGenerator == null) enemyGenerator = GetComponent<EnemyGenerator>();
            if (objectsGenerator == null) objectsGenerator = GetComponent<ObjectsGenerator>();
        }

        public void ClearDungeon()
        {
            InitComponents();

            // cleanup any active objects
            objectsGenerator?.DestroyObjects();

            // Cleanup objects
            enemyGenerator?.DestroyEnemies();

            // Cleanup tilemap
            dungeonVisualizer?.Clear();

            // Cleanup any remaining child gameobject
            GameObjectHelper.DestroyChildren(transform);

            disableGenerateOnStart = false;
        }

        public void GenerateDungeon()
        {
            InitComponents();

            ClearDungeon();
            CreateDungeon();

            disableGenerateOnStart = true;
        }

        private void CreateDungeon()
        {
            var roomVisualiser = new RoomVisualizer(dungeonVisualizer);
            var roomManager = new RoomManager(roomVisualiser);
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


            var rooms = roomManager.GetRooms();
            PositionPlayerCharacter(rooms.First().RoomCenter);

            GenerateObjects(rooms);
            GenerateEnemies(rooms);

        }

        private void PositionPlayerCharacter(Vector2Int position)
        {
            playerCharacter.transform.position = (Vector3Int)position;
        }

        public void GenerateEnemies(IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.GetRoomType() == RoomType.Default)
                {
                    enemyGenerator.GenerateEnemies(room, 4);
                }
                if (room.GetRoomType() == RoomType.BossRoom)
                {
                    enemyGenerator.GenerateBossEnemy(room);
                }
            }
        }

        public void GenerateObjects(IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.GetRoomType() == RoomType.BossRoom)
                {
                    objectsGenerator.GenerateExit(room);
                }
            }
        }

    }
}