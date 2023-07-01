using Assets.Scripts.Characters.Shared;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class BaseSpell : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;

        protected PlayerController playerController;
        protected SpriteRenderer spriteRenderer;
        protected Movement movement;

        protected virtual void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            movement = new Movement(GetComponent<Rigidbody2D>());
            movement.SetMoveSpeed(6f);
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
            if (Vector3.Distance(playerController.GetSpriteCenter(), spriteRenderer.bounds.center) < range)
            {
                playerController.TakeDamage(damage);
                return true;
            }

            return false;
        }
    }
}