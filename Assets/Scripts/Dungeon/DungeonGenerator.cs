using Assets.Scripts.Dungeon.Corridors;
using Assets.Scripts.Dungeon.Rooms;
using Assets.Scripts.Helpers;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    [RequireComponent(typeof(DungeonVisualizer))]
    [RequireComponent(typeof(EnemyGenerator))]
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        GameObject playerCharacter;

        [SerializeField]
        Vector2Int roomCount;

        [SerializeField]
        public int minRoomCount = 6;

        [SerializeField]
        public int maxRoomCount = 8;

        [SerializeField]
        bool disableGenerateOnStart = false;

        private DungeonVisualizer dungeonVisualizer;
        private RoomManager roomManager;
        private CorridorManager corridorManager;
        private EnemyGenerator enemyGenerator;

        private void Start()
        {
            if (!disableGenerateOnStart)
                GenerateDungeon();
        }

        private void InitComponents()
        {
            if (dungeonVisualizer == null) dungeonVisualizer = GetComponent<DungeonVisualizer>();
            if (roomManager == null) roomManager = GetComponent<RoomManager>();
            if (corridorManager == null) corridorManager = GetComponent<CorridorManager>();
            if (enemyGenerator == null) enemyGenerator = GetComponent<EnemyGenerator>();
        }

        public void ClearDungeon()
        {
            InitComponents();

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
            if (!Application.isPlaying)
            {
                corridorManager.Awake();
            }

            enemyGenerator.RandomizePossibleLevelEnemies(2);
            roomManager.GenerateRooms(minRoomCount, maxRoomCount);
            corridorManager.GenerateCorridors();
            dungeonVisualizer.SetAllFloorTiles();

            var rooms = roomManager.GetRooms();
            PositionPlayerCharacter(rooms.First().Floor.Center);
        }

        private void PositionPlayerCharacter(Vector2Int position)
        {
            playerCharacter.transform.position = (Vector3Int)position;
        }
    }
}