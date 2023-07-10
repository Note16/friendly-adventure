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

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            StartCoroutine(HitEnemyAfterDelay(0.3f));
            StartCoroutine(SoundEffectAfterDelay(0.3f));
            StartCoroutine(HitEnemyAfterDelay(0.5f));
            StartCoroutine(DisableAfterDelay(0.8f));
        }

        IEnumerator SoundEffectAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (AttackSounds.Count > 0)
            {
                SoundManager.instance.RandomizeSfx(AttackSounds.ToArray());
            }
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