using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Shared;
using Assets.Scripts.Helpers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float CombatTextYAxis = 1f;

        [SerializeField]
        private int healthPoints = 10;

        [SerializeField]
        private int damage = 2;

        [SerializeField]
        private float meleeAttackRange = 4f;

        [SerializeField]
        private bool spriteIsFacingRight = true;

        private PlayerController playerController;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Movement movement;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            movement = new Movement(GetComponent<Rigidbody2D>());
            playerController = FindObjectOfType<PlayerController>();
        }

        public void FixedUpdate()
        {
            // Update sorting order so enemy shows on top of player
            // When player is positioned above the enemy
            var relativePos = (Vector2)playerController.transform.position - (Vector2)transform.position;
            spriteRenderer.sortingOrder = relativePos.y < -1 ? 0 : 2;

            TryMeleeAttack();
        }

        public void TryMeleeAttack()
        {
            // When player is above the enemy half the melee radius
            var attackRange = (spriteRenderer.sortingOrder > 0) ? meleeAttackRange - 2 : meleeAttackRange;

            if (Vector3.Distance(playerController.transform.position, transform.position) < attackRange)
            {
                var currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
                if (!currentAnimation.IsName("TakeHit") && !currentAnimation.IsName("Death"))
                {
                    animator.Play("Attack1");
                }
            }
        }

        public void Damage(int damage)
        {
            var isCrit = RandomHelper.GetRandom(50);
            if (isCrit)
                damage *= 2;

            healthPoints -= damage;

            DamagePopup.Create(transform, CombatTextYAxis, damage, isCrit);

            if (healthPoints <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");
        }

        public void Move(Vector2 targetPosition)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeHit"))
                return;

            var moveInput = (targetPosition - (Vector2)transform.position).normalized;
            var moveSuccess = movement.Move(moveInput);
            animator.SetBool("isMoving", moveSuccess);


            if (spriteIsFacingRight)
            {
                // If we are moving left flip sprite!
                spriteRenderer.flipX = moveInput.x < 0;
            }
            else
            {
                // If we are moving right flip sprite!
                spriteRenderer.flipX = moveInput.x > 0;
            }
        }

        public void AnimationEvent(string command)
        {
            if (command == "Attack1")
            {
                // Check if still in melee range
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    playerController.Damage(damage);
                }
            }
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