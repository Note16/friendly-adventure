using UnityEngine;

namespace Assets.Scripts.Dungeon.Rooms
{
    [RequireComponent(typeof(RoomObjectsGenerator))]
    [RequireComponent(typeof(RoomEnemyGenerator))]
    public class Room : MonoBehaviour
    {
        [HideInInspector]
        public RectInt Rect;
        private RoomObjectsGenerator objectsGenerator;
        private RoomEnemyGenerator enemiesGenerator;
        public RoomVisualizer RoomVisualizer { get; private set; }
        public RoomWalls Walls { get; private set; }
        public RoomFloor Floor { get; private set; }
        [SerializeField]
        public RoomType RoomType;
        public Vector2Int RectCenter { get; private set; }

        private int wallHeight = 5;
        private int pillarDistance = 4;

        public void Awake()
        {
            objectsGenerator = GetComponent<RoomObjectsGenerator>();
            enemiesGenerator = GetComponent<RoomEnemyGenerator>();
            RoomVisualizer = new RoomVisualizer(GetComponentInParent<DungeonVisualizer>());

            if (!Application.isPlaying)
            {
                objectsGenerator.Awake();
                enemiesGenerator.Awake();
            }
        }

        public void Create()
        {
            Walls = new RoomWalls(Rect, wallHeight, pillarDistance);
            Floor = new RoomFloor(Rect, Walls);
            RectCenter = Vector2Int.FloorToInt(Rect.center);

            Floor.Render(RoomVisualizer);
            Walls.Render(RoomVisualizer);

            if (RoomType == RoomType.BossRoom)
            {
                enemiesGenerator.GenerateBossEnemy();
                objectsGenerator.GenerateExit();
                Walls.CreateExit(RoomVisualizer);
            }
            if (RoomType == RoomType.Default)
            {
                enemiesGenerator.GenerateEnemies();
                objectsGenerator.GenerateWallObjects();
                objectsGenerator.GenerateFloorObjects();
            }
            if (RoomType == RoomType.Shop || RoomType == RoomType.Entrance)
            {
                objectsGenerator.GenerateWallObjects();
                objectsGenerator.GenerateFloorObjects();
            }
        }
    }
}