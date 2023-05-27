using Assets.Scripts.Dungeon.Areas.Rooms;
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

        private List<GameObject> activeObjects;

        public ObjectsGenerator()
        {
            activeObjects = new List<GameObject>();
        }

        public void GenerateExit(Room room)
        {
            var exitObj = dungeonObjects.FirstOrDefault(obj => obj.Name == "Exits")?.GameObjects.FirstOrDefault();
            var position = new Vector2Int(room.Rect.xMax - 4, room.Walls.Top.yMin + 3);
            activeObjects.Add(GameObject.Instantiate(exitObj, (Vector3Int)position, Quaternion.identity, transform));
        }

        public void DestroyObjects()
        {
            if (activeObjects.Any())
                GameObjectHelper.Destroy(activeObjects);
        }
    }
}