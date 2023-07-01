using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        protected float moveSpeed = 4f;

        [SerializeField]
        private float collisionOffset = 0.5f;

        private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
        private ContactFilter2D moveFilter;
        private Rigidbody2D rb;
        protected bool stop = false;

        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Stop(bool stop)
        {
            this.stop = stop;
        }

        public bool Move(Vector2 direction)
        {
            if (stop)
                return false;

            // Lets try moving
            var moveSuccess = TryMove(direction);

            // We couldn't move..
            if (!moveSuccess)
            {
                // Lets try moving horizontal
                moveSuccess = TryMove(new Vector2(direction.x, 0));

                // We still couldn't move..
                if (!moveSuccess)
                {
                    // Lets try moving vertical
                    moveSuccess = TryMove(new Vector2(0, direction.y));
                }
            }

            return moveSuccess;
        }

        public bool SimpleMove(Vector2 direction)
        {
            if (stop)
                return false;

            // Lets try moving
            return TryMove(direction);
        }

        public void MoveIgnoreCollision(Vector2 direction)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        private bool TryMove(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                // Check for collision
                int count = rb.Cast(
                    direction,
                    moveFilter,
                    collisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);

                if (count == 0)
                {
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                    return true;
                }
            }
            return false;
        }

        public void Pushed(Vector2 direction)
        {
            var newPosition = rb.position + direction;

            // Check for collisions
            rb.Cast(
                direction,
                collisions,
                Vector2.Distance(rb.position, newPosition));

            // Check for wall collision
            if (!collisions.Any(rayhit => rayhit.collider.name == "Walls"))
                rb.MovePosition(newPosition);
        }
    }
}