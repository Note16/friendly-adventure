using UnityEngine;

namespace Assets.Scripts.Dungeon.Rooms
{
    [RequireComponent(typeof(RoomObjectsGenerator))]
    public class Room : MonoBehaviour
    {
        [SerializeField]
        public RectInt Rect;

        private RoomObjectsGenerator objectsGenerator;
        public RoomVisualizer RoomVisualizer { get; private set; }
        public RoomWalls Walls { get; private set; }
        public RoomFloor Floor { get; private set; }
        public RoomType RoomType { get; private set; }
        public Vector2Int RectCenter { get; private set; }

        private int wallHeight = 5;
        private int pillarDistance = 4;

        public void Awake()
        {
            objectsGenerator = GetComponent<RoomObjectsGenerator>();

            if (!Application.isPlaying)
                objectsGenerator.Awake();
        }

        public void SetRoomVisualizer(RoomVisualizer roomVisualizer)
        {
            RoomVisualizer = roomVisualizer;
        }

        public void SetRoomType(RoomType type)
        {
            RoomType = type;

            if (RoomType == RoomType.BossRoom)
            {
                Walls.CreateExit(RoomVisualizer);
            }
        }

        public void Create()
        {
            Walls = new RoomWalls(Rect, wallHeight, pillarDistance);
            Floor = new RoomFloor(Rect, Walls);
            RectCenter = Vector2Int.FloorToInt(Rect.center);

            Floor.Render(RoomVisualizer);
            Walls.Render(RoomVisualizer);
        }


        public void GenerateObjects()
        {
            if (RoomType == RoomType.BossRoom)
            {
                objectsGenerator.GenerateExit();
            }
            else if (RoomType == RoomType.Default)
            {
                objectsGenerator.GenerateWallObjects();
                objectsGenerator.GenerateFloorObjects();
            }
        }
    }
}