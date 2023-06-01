using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Shared;
using Assets.Scripts.Helpers;
using Assets.Scripts.UI;
using System.Linq;
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
        private int Attack1Damage = 2;

        [SerializeField]
        private int Attack2Damage = 4;

        [SerializeField]
        private float meleeAttackRange = 4f;

        [SerializeField]
        private float castingAttackRange = 8f;

        [SerializeField]
        private bool spriteIsFacingRight = true;

        [SerializeField]
        private GameObject Spell1;

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

            TryAttack();
        }

        public void TryAttack()
        {

            if (!AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2", "Cast" }))
            {
                if (Spell1 != null && Vector3.Distance(playerController.transform.position, transform.position) < castingAttackRange)
                {
                    if (RandomHelper.GetRandom(2))
                    {
                        animator.Play("Cast");
                    }
                }
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    var attack = RandomHelper.GetRandom(40) ? "Attack2" : "Attack1";
                    animator.Play(attack);
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

        public void Push(Vector2 direction)
        {
            movement.Pushed(direction);
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
                    playerController.Damage(Attack1Damage);
                }
            }
            if (command == "Attack2")
            {
                // Check if still in melee range
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    playerController.Damage(Attack2Damage);
                }
            }
            if (command == "Cast")
            {
                Instantiate(Spell1, playerController.transform.position, Quaternion.identity);
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

        public bool AnimationIsPlaying(string[] name)
        {
            var currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
            return name.Any(name => currentAnimation.IsName(name));
        }
    }
}