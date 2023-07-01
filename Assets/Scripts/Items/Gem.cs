using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [RequireComponent(typeof(Movement))]
    public class Gem : MonoBehaviour
    {
        [SerializeField]
        private int value = 1;

        private Movement movement;

        void Start()
        {
            movement = GetComponent<Movement>();
        }

        public void Move(Vector3 targetPosition)
        {
            var moveInput = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
            movement.MoveIgnoreCollision(moveInput);
        }

        public int PickUp()
        {
            Destroy(gameObject);
            return value;
        }
    }
}