using Assets.Scripts.Dungeon.Rooms;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    [RequireComponent(typeof(Room))]
    public class RoomObjectsGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject exitObject;

        [SerializeField]
        private List<GameObject> wallObjects;

        [SerializeField]
        private List<GameObject> floorObjects;

        private HashSet<GameObject> activeObjects;
        private Room room;

        public void Awake()
        {
            room = GetComponent<Room>();
            activeObjects = new HashSet<GameObject>();
        }

        public void GenerateWallObjects()
        {
            if (wallObjects?.Any() == true)
            {
                var wallItem = RandomHelper.GetRandom(wallObjects);
                var leftPosition = new Vector2Int(room.Rect.xMin + 4, room.Walls.Top.yMin + 3);
                activeObjects.Add(Instantiate(wallItem, (Vector3Int)leftPosition, Quaternion.identity, room.transform));

                var rightPosition = new Vector2Int(room.Rect.xMax - 4, room.Walls.Top.yMin + 3);
                activeObjects.Add(Instantiate(wallItem, (Vector3Int)rightPosition, Quaternion.identity, room.transform));
            }
        }

        public void GenerateFloorObjects()
        {
            if (floorObjects?.Any() == true)
            {
                var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();

                var itemCount = 8;

                for (int i = 0; i < itemCount; i++)
                {
                    var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);

                    // Do not generate object if on already exists in < 1f distance
                    if (!activeObjects.Any(obj => Vector3.Distance(obj.transform.position, randomPosition) < 1f))
                        activeObjects.Add(Instantiate(RandomHelper.GetRandom(floorObjects), randomPosition, Quaternion.identity, room.transform));
                }
            }
        }

        public void GenerateExit()
        {
            var position = new Vector2Int(room.Rect.xMax - 4, room.Walls.Top.yMin + 3);
            var exit = Instantiate(exitObject, (Vector3Int)position, Quaternion.identity, room.transform);
            exit.SetActive(false);
            activeObjects.Add(exit);
        }

        public void DestroyObjects()
        {
            if (activeObjects.Any())
            {
                GameObjectHelper.Destroy(activeObjects);
                activeObjects.Clear();
            }
        }
    }
}