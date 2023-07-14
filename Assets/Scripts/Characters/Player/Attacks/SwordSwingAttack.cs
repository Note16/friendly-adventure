using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Sounds;
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

        [SerializeField]
        public List<AudioClip> AttackSounds;

        [SerializeField]
        public List<AudioClip> AttackSoundsHit;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(HitEnemyAfterDelayWithSound(0.3f));
            StartCoroutine(HitEnemyAfterDelay(0.5f));
            StartCoroutine(DisableAfterDelay(0.8f));
        }

        IEnumerator HitEnemyAfterDelayWithSound(float delay)
        {
            yield return new WaitForSeconds(delay);

            HitEnemy();

        }

        IEnumerator HitEnemyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (AttackSounds.Count > 0)
            {
                SoundManager.instance.RandomizeSfx(AttackSounds.ToArray());
            }
            HitEnemy();
        }

        IEnumerator DisableAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }

        private bool HitEnemy()
        {
            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            var enemyHit = false;
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
                {
                    enemy.TakeDamage(damage);

                    var direction = (enemy.transform.position - transform.position) / 2;
                    enemy.Push((Vector2)direction);

                    if (AttackSoundsHit.Count > 0)
                    {
                        SoundManager.instance.RandomizeSfx(AttackSoundsHit.ToArray());
                    }
                }
            }

            return enemyHit;
        }
    }
}