using Assets.Scripts.Characters.Shared;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerMovement : Movement
    {
        private readonly Animator animator;
        private readonly SpriteRenderer spriteRenderer;

        public PlayerMovement(Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rigidbody2D) : base(rigidbody2D)
        {
            this.animator = animator;
            this.spriteRenderer = spriteRenderer;
        }

        public void Animate(Vector2 direction)
        {
            if (stop)
                return;

            // We are standing still
            if (direction == Vector2.zero)
            {
                // Set is moving animation parameters
                animator.SetBool("isMovingHorizontal", false);
                animator.SetBool("isMovingVertical", false);
                animator.SetBool("isMoving", false);

                // exit code
                return;
            }

            // If we are moving left flip sprite!
            spriteRenderer.flipX = direction.x < 0;

            // Set is moving animation parameters
            animator.SetBool("isMovingHorizontal", direction.x != 0);
            animator.SetBool("isMovingVertical", direction.y != 0);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
    }
}