using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class PentagramAttack : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;
        private Rigidbody2D rb;

        [SerializeField]
        public List<AudioClip> AttackSounds;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            StartCoroutine(SoundEffectAfterDelay(0.15f));
            StartCoroutine(HitEnemyAfterDelay(0.3f));
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
                }
            }
        }
    }
}