using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(Movement))]
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField]
        private int heal = 6;

        private Rigidbody2D rb;
        private Movement movement;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<Movement>();
        }

        public void Move(Vector3 targetPosition)
        {
            var moveInput = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
            movement.MoveIgnoreCollision(moveInput);
        }

        public int Consume()
        {
            Destroy(gameObject);
            return heal;
        }
    }
}