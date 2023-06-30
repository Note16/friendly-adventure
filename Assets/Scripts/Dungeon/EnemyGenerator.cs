using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> possibleEnemies;
        [SerializeField]
        private GameObject bossEnemy;

        private List<GameObject> enemies;

        public EnemyGenerator()
        {
            enemies = new List<GameObject>();
        }

        public void GenerateBossEnemy(Room room)
        {
            enemies.Add(Instantiate(bossEnemy, (Vector3Int)room.Floor.Center, Quaternion.identity, gameObject.transform));
        }

        public void GenerateEnemies(Room room, int count)
        {
            var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();

            for (int i = 0; i < count; i++)
            {
                var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);

                var enemy = Instantiate(RandomHelper.GetRandom(possibleEnemies), randomPosition, Quaternion.identity, transform);
                var spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                spriteRenderer.flipX = RandomHelper.GetRandom(50);

                enemies.Add(enemy);
            }
        }

        public void DestroyEnemies()
        {
            if (enemies.Any())
                GameObjectHelper.Destroy(enemies);
        }
    }
}