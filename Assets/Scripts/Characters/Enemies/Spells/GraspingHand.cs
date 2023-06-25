using Assets.Scripts.Characters.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class GraspingHand : MonoBehaviour
    {
        [SerializeField]
        public int damage = 4;
        private Rigidbody2D rb;
        private Movement movement;
        private PlayerController playerController;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            movement = new Movement(rb);
            movement.SetMoveSpeed(6f);
            playerController = FindObjectOfType<PlayerController>();

            StartCoroutine(HitEnemyAfterDelay(0.5f));
            StartCoroutine(HitEnemyAfterDelay(0.6f));
        }

        private void FixedUpdate()
        {
            Move(playerController.transform.position);
        }

        public void Move(Vector2 targetPosition)
        {
            var moveInput = (targetPosition - (Vector2)transform.position).normalized;
            movement.MoveIgnoreCollision(moveInput);
        }

        IEnumerator HitEnemyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<PlayerController>(out var player))
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}