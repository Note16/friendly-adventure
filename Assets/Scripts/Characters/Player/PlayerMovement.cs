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

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void Animate(Vector2 moveInput)
        {
            if (stop)
                return;

            // We are standing still
            if (moveInput == Vector2.zero)
            {
                // Set is moving animation parameters
                animator.SetBool("isMovingHorizontal", false);
                animator.SetBool("isMovingVertical", false);
                animator.SetBool("isMoving", false);

                // exit code
                return;
            }

            // If we are moving left flip sprite!
            spriteRenderer.flipX = moveInput.x < 0;

            // Set is moving animation parameters
            animator.SetBool("isMovingHorizontal", moveInput.x != 0);
            animator.SetBool("isMovingVertical", moveInput.y != 0);
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
        }
    }
}