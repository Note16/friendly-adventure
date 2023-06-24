using Assets.Scripts.Characters.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Attacks
{
    public class GhostProjectile : MonoBehaviour
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

            Destroy(gameObject, 1f);
        }

        private void FixedUpdate()
        {
            Move(playerController.transform.position - new Vector3(0, -0.6f, 0));
            HitEnemy();
        }

        private void Update()
        {
            transform.rotation = Quaternion.FromToRotation(transform.position, playerController.transform.position - transform.position - new Vector3(0, -1, 0));
        }


        public void Move(Vector3 targetPosition)
        {
            var moveInput = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
            movement.MoveIgnoreCollision(moveInput);
        }

        void HitEnemy()
        {
            var hits = new List<RaycastHit2D>();
            rb.Cast(Vector2.zero, hits);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<PlayerController>(out var player))
                {
                    player.Damage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}