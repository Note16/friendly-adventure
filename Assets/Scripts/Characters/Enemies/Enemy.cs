using Assets.Scripts.Characters.Shared;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private int healthPoints = 10;

        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Movement movement;


        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            movement = new Movement(GetComponent<Rigidbody2D>());
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
            Debug.Log(moveInput);
            var moveSuccess = movement.SimpleMove(moveInput);
            animator.SetBool("isMoving", moveSuccess);

            // If we are moving left flip sprite!
            spriteRenderer.flipX = moveInput.x < 0;
        }

        public void StopMovement()
        {
            animator.SetBool("isMoving", false);
        }
    }
}