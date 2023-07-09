using Assets.Scripts.Shared;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    [RequireComponent(typeof(Movement))]
    public class SpellBase : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;

        protected PlayerStats playerStats;
        protected SpriteRenderer spriteRenderer;
        protected Movement movement;

        protected virtual void Awake()
        {
            playerStats = FindObjectOfType<PlayerStats>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<Movement>();
        }

        protected void Move(Vector2 targetPosition)
        {
            var moveInput = (targetPosition - (Vector2)transform.position).normalized;
            movement.MoveIgnoreCollision(moveInput);
        }

        protected IEnumerator HitEnemyAfterDelay(float delay, float range)
        {
            yield return new WaitForSeconds(delay);

            HitEnemy(range);
        }

        protected bool HitEnemy(float range)
        {
            if (Vector3.Distance(playerStats.GetSpriteCenter(), spriteRenderer.bounds.center) < range)
            {
                playerStats.TakeDamage(damage);
                return true;
            }

            return false;
        }
    }
}