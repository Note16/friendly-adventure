using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Characters.Enemies.Versions;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> possibleEnemies;
        private List<GameObject> possibleLevelEnemies;

        private void Awake()
        {
            possibleLevelEnemies = possibleEnemies;
        }

        public void RandomizePossibleLevelEnemies(int maxTypesCount)
        {
            possibleLevelEnemies = new List<GameObject>();
            for (int i = 0; i < maxTypesCount; i++)
            {
                possibleLevelEnemies.Add(RandomHelper.GetRandom(possibleEnemies));
            }
        }

        public GameObject GenerateBossEnemy(Vector3Int position, Transform parent)
        {
            var bossEnemy = Instantiate(RandomHelper.GetRandom(possibleLevelEnemies), position, Quaternion.identity, parent);
            bossEnemy.transform.localScale = new Vector3(10, 10, 0);
            bossEnemy.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
            if (bossEnemy.TryGetComponent<BaseEnemy>(out var baseEnemy))
            {
                baseEnemy.healthPoints *= 5;
                baseEnemy.itemDropMultiplier *= 10;
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

            return bossEnemy;
        }

        public GameObject GenerateEnemy(Vector3Int position, Transform parent)
        {
            var enemy = Instantiate(RandomHelper.GetRandom(possibleLevelEnemies), position, Quaternion.identity, parent);
            var spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = RandomHelper.GetRandom(50);
            return enemy;
        }
    }
}