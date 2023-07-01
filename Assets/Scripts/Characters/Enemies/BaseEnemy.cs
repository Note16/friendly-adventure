using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Shared;
using Assets.Scripts.Helpers;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]
        protected bool spriteIsFacingRight = true;

        [SerializeField]
        protected float combatTextYAxis = 1f;

        [SerializeField]
        public int healthPoints = 10;

        [SerializeField]
        protected List<GameObject> itemDrops = new List<GameObject>();

        public int itemDropMultiplier = 1;

        protected PlayerController playerController;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Movement movement;

        public Action OnDeathAction;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            movement = new Movement(GetComponent<Rigidbody2D>());
            playerController = FindObjectOfType<PlayerController>();
        }

        private void FixedUpdate()
        {
            // Update sorting order so enemy shows on top of player
            // When player is positioned above the enemy
            var relativePos = (Vector2)playerController.transform.position - (Vector2)transform.position;
            var layer = Mathf.CeilToInt(relativePos.y * 2);
            spriteRenderer.sortingOrder = relativePos.y < 0f ? layer : layer + 1;

            TryAttack();
        }

        public virtual void TryAttack()
        {
            // Implement Attack
        }

        public void TakeDamage(int damage)
        {
            var isCrit = RandomHelper.GetRandom(50);
            if (isCrit)
                damage *= 2;

            healthPoints -= damage;

            CombatTextPopup.Damage(transform, combatTextYAxis, damage, isCrit);

            if (healthPoints <= 0)
            {
                DropItems();
                OnDeathAction?.Invoke();
                animator.Play("Death"); // Will call DestroyOnExit behavior script
            }
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

        public void StopMovement()
        {
            animator.SetBool("isMoving", false);
        }

        protected virtual void AnimationEvent(string command)
        {
            if (command == "StopMovement")
                movement.Stop(true);

            if (command == "ContinueMovement")
                movement.Stop(false);
        }

        protected bool AnimationIsPlaying(string[] name)
        {
            var currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
            return name.Any(name => currentAnimation.IsName(name));
        }

        private void DropItems()
        {
            if (!itemDrops.Any())
                return;

            for (int i = 0; i < itemDropMultiplier; i++)
            {
                var dropZone = new Bounds(transform.position, new Vector3(2, 2));
                Instantiate(RandomHelper.GetRandom(itemDrops), RandomHelper.GetRandom(dropZone), Quaternion.identity, transform.parent);
            }
        }
    }
}