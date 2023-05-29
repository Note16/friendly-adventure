using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Shared
{
    public class Movement
    {
        protected float moveSpeed = 3f;
        protected bool stop = false;

        private readonly Rigidbody2D rigidbody2D;
        private ContactFilter2D moveFilter;
        private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
        private float collisionOffset = 0.5f;

        public Movement(Rigidbody2D rigidbody2D)
        {
            this.rigidbody2D = rigidbody2D;
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
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
            rigidbody2D.MovePosition(rigidbody2D.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        private bool TryMove(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                // Check for collision
                int count = rigidbody2D.Cast(
                    direction,
                    moveFilter,
                    collisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);

                if (count == 0)
                {
                    rigidbody2D.MovePosition(rigidbody2D.position + direction * moveSpeed * Time.fixedDeltaTime);
                    return true;
                }
            }
            return false;
        }
    }
}