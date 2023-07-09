﻿using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Rooms
{
    [RequireComponent(typeof(Room))]
    public class RoomEnemyGenerator : MonoBehaviour
    {
        [SerializeField]
        int enemyCount = 4;

        private Room room;
        private EnemyGenerator enemyGenerator;

        public void Awake()
        {
            room = GetComponent<Room>();
            enemyGenerator = gameObject.GetComponentInParent<EnemyGenerator>();
        }

        public void GenerateEnemies()
        {
            var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();
            for (int i = 0; i < enemyCount; i++)
            {
                var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);
                enemyGenerator.GenerateEnemy(randomPosition, transform);
            }
        }

        public void GenerateBossEnemy()
        {
            if (enemyCount == 1)
                enemyGenerator.GenerateBossEnemy((Vector3Int)room.Floor.Center, transform);
            else
            {
                var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();
                for (int i = 0; i < enemyCount; i++)
                {
                    var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);
                    enemyGenerator.GenerateBossEnemy(randomPosition, transform);
                }
            }
        }
    }
}