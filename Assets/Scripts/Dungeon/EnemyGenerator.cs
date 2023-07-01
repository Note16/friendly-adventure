using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Dungeon.Areas.Rooms;
using Assets.Scripts.Dungeon.Objects;
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
        private List<GameObject> possibleLevelEnemies;

        private List<GameObject> enemies;

        public EnemyGenerator()
        {
            enemies = new List<GameObject>();
            possibleLevelEnemies = possibleEnemies;
        }

        public void RandomizeLevelEnemies(int maxTypesCount)
        {
            possibleLevelEnemies = new List<GameObject>();
            for (int i = 0; i < maxTypesCount; i++)
            {
                possibleLevelEnemies.Add(RandomHelper.GetRandom(possibleEnemies));
            }
        }

        public void GenerateBossEnemy(Room room)
        {
            var bossEnemy = Instantiate(RandomHelper.GetRandom(possibleLevelEnemies), (Vector3Int)room.Floor.Center, Quaternion.identity, gameObject.transform);
            bossEnemy.transform.localScale = new Vector3(10, 10, 0);
            bossEnemy.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
            if (bossEnemy.TryGetComponent<BaseEnemy>(out var baseEnemy))
            {
                baseEnemy.healthPoints *= 5;
                baseEnemy.itemDropMultiplier *= 10;
                baseEnemy.OnDeathAction = () =>
                {
                    var exit = FindObjectOfType<Exit>(true);
                    exit.gameObject.SetActive(true);
                };
            }
            if (bossEnemy.TryGetComponent<MeleeEnemy>(out var meleeEnemy))
            {
                meleeEnemy.Attack1Damage *= 3;
                meleeEnemy.Attack2Damage *= 3;
            }
            if (bossEnemy.TryGetComponent<CasterEnemy>(out var casterEnemy))
            {
                var spellbook = casterEnemy.GetComponent<SpellBook>();
                spellbook.spellSizeMultiplier *= 2;
                spellbook.spellDamageMultiplier *= 3;
            }
            if (bossEnemy.TryGetComponent<WarCasterEnemy>(out var warCasterEnemy))
            {
                warCasterEnemy.Attack1Damage *= 3;
                warCasterEnemy.Attack2Damage *= 3;

                var spellbook = warCasterEnemy.GetComponent<SpellBook>();
                spellbook.spellSizeMultiplier *= 2;
                spellbook.spellDamageMultiplier *= 3;
            }

            enemies.Add(bossEnemy);
        }

        public void GenerateEnemies(Room room, int count)
        {
            var roomFloor = room.Floor.Inner.allPositionsWithin.ToVector2Int();

            for (int i = 0; i < count; i++)
            {
                var randomPosition = (Vector3Int)RandomHelper.GetRandom(roomFloor);

                var enemy = Instantiate(RandomHelper.GetRandom(possibleLevelEnemies), randomPosition, Quaternion.identity, transform);
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