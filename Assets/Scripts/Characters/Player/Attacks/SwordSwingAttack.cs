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

            StartCoroutine(HitEnemyAfterDelay(0.3f));
            StartCoroutine(HitEnemyAfterDelay(0.5f));
        }

        IEnumerator HitEnemyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Damage(damage);

                    var direction = (enemy.transform.position - transform.position) / 2;
                    enemy.Push((Vector2)direction);
                }
            }
        }
    }
}