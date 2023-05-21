using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Shared;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int healthPoints = 10;

        [SerializeField]
        private int damage = 2;

        [SerializeField]
        private bool allowSpriteFlip = true;

        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Movement movement;

        private PlayerController playerCharacter;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            movement = new Movement(GetComponent<Rigidbody2D>());
        }

        public void FixedUpdate()
        {
            var collisions = movement.GetCollisions();

            var playerCollision = collisions.FirstOrDefault(collision => collision.collider.GetComponent<PlayerController>());

            if (playerCollision)
            {
                playerCharacter = playerCollision.collider.GetComponent<PlayerController>();

                animator.Play("Attack1");
            }
        }

        public void Damage(int damage)
        {
            healthPoints -= damage;

            if (healthPoints <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");
        }

        public void Move(Vector2 targetPosition)
        {
            var moveInput = (targetPosition - (Vector2)transform.position).normalized;
            var moveSuccess = movement.SimpleMove(moveInput);
            animator.SetBool("isMoving", moveSuccess);

            if (allowSpriteFlip)
            {
                // If we are moving left flip sprite!
                spriteRenderer.flipX = moveInput.x < 0;
            }
        }

        public void AnimationEvent(string command)
        {
            if (command == "Attack1" && playerCharacter != null)
                playerCharacter.Damage(damage);
            if (command == "StopMovement")
                movement.Stop(true);
            if (command == "ContinueMovement")
                movement.Stop(false);
        }

        public void StopMovement()
        {
            animator.SetBool("isMoving", false);
        }
    }
}