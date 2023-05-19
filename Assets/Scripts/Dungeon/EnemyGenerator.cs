﻿using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class EnemyGenerator : MonoBehaviour
    {
        public List<GameObject> PossibleEnemies;
        public GameObject BossEnemy;

        private List<GameObject> Enemies;

        public EnemyGenerator()
        {
            Enemies = new List<GameObject>();
        }

        public void GenerateBossEnemy(Room room)
        {
            Enemies.Add(Instantiate(BossEnemy, (Vector3Int)room.RoomCenter, Quaternion.identity, gameObject.transform));
        }

        public void GenerateEnemies(Room room, int count)
        {
            var roomFloor = room.InnerFloor.allPositionsWithin.ToVector2Int();

            for (int i = 0; i < count; i++)
            {
                var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);

                var enemy = Instantiate(RandomHelper.GetRandom(PossibleEnemies), randomPosition, Quaternion.identity, gameObject.transform);
                var spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                spriteRenderer.flipX = RandomHelper.GetRandom(50);

                Enemies.Add(enemy);
            }
        }

        public void DestroyEnemies()
        {
            foreach (var enemy in Enemies)
            {
                if (Application.isEditor)
                    DestroyImmediate(enemy);
                else
                    Destroy(enemy);
            }
        }
    }
}