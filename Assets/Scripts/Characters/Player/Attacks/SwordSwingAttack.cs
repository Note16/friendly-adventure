using Assets.Scripts.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class SwordSwingAttack : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;
        private Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(HitEnemyAfterDelay(0.3f));
            StartCoroutine(HitEnemyAfterDelay(0.5f));
            StartCoroutine(DisableAfterDelay(0.8f));
        }

        IEnumerator HitEnemyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
                {
                    enemy.TakeDamage(damage);

                    var direction = (enemy.transform.position - transform.position) / 2;
                    enemy.Push((Vector2)direction);
                }
            }
        }

        IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}