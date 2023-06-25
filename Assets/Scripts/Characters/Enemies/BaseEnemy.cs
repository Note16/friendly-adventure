﻿using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Shared;
using Assets.Scripts.Helpers;
using Assets.Scripts.UI;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField]
        protected bool spriteIsFacingRight = true;

        [SerializeField]
        protected float CombatTextYAxis = 1f;

        [SerializeField]
        protected int healthPoints = 10;

        protected PlayerController playerController;
        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Movement movement;

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

            var pivitPoint = spriteRenderer.size.y - 0.64f;
            spriteRenderer.sortingOrder = relativePos.y < pivitPoint ? 0 : 2;

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
    }
}