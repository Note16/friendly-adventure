using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    [Serializable]
    public class NamedGameObject
    {
        public string Name;
        public List<GameObject> GameObjects;
    }

    public class ObjectsGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<NamedGameObject> dungeonObjects;

        private HashSet<GameObject> activeObjects;

        public ObjectsGenerator()
        {
            activeObjects = new HashSet<GameObject>();
        }

        public void GenerateWallObjects(Room room)
        {
            var wallItems = dungeonObjects.FirstOrDefault(obj => obj.Name == "Wall");
            if (wallItems != null)
            {
                var wallItem = RandomHelper.GetRandom(wallItems.GameObjects);
                var leftPosition = new Vector2Int(room.Rect.xMin + 4, room.Walls.Top.yMin + 3);
                activeObjects.Add(Instantiate(wallItem, (Vector3Int)leftPosition, Quaternion.identity, transform));

                var rightPosition = new Vector2Int(room.Rect.xMax - 4, room.Walls.Top.yMin + 3);
                activeObjects.Add(Instantiate(wallItem, (Vector3Int)rightPosition, Quaternion.identity, transform));
            }
        }

        public void GenerateFloorObjects(Room room)
        {
            var floorItems = dungeonObjects.FirstOrDefault(obj => obj.Name == "Floor");
            if (floorItems != null)
            {
                var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();

                var itemCount = 8;

                for (int i = 0; i < itemCount; i++)
                {
                    var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);

                    // Do not generate object if on already exists in < 1f distance
                    if (!activeObjects.Any(obj => Vector3.Distance(obj.transform.position, randomPosition) < 1f))
                        activeObjects.Add(Instantiate(RandomHelper.GetRandom(floorItems.GameObjects), randomPosition, Quaternion.identity, transform));
                }
            }
        }

        public void GenerateExit(Room room)
        {
            var exitObj = dungeonObjects.FirstOrDefault(obj => obj.Name == "Exits")?.GameObjects.FirstOrDefault();
            var position = new Vector2Int(room.Rect.xMax - 4, room.Walls.Top.yMin + 3);
            activeObjects.Add(Instantiate(exitObj, (Vector3Int)position, Quaternion.identity, transform));
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