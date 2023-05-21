using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Shared
{
    public class Movement
    {
        protected float moveSpeed = 0.5f;
        protected bool stop = false;

        private readonly Rigidbody2D rigidbody2D;
        private ContactFilter2D moveFilter;
        private List<RaycastHit2D> collisions = new List<RaycastHit2D>();
        private float collisionOffset = 0.05f;

        public Movement(Rigidbody2D rigidbody2D)
        {
            this.rigidbody2D = rigidbody2D;
        }

        public void Stop(bool stop)
        {
            this.stop = stop;
        }

        public void Move(Vector2 moveInput)
        {
            if (stop)
                return;

            // Lets try moving
            var moveSuccess = TryMove(moveInput);

            // We couldn't move..
            if (!moveSuccess)
            {
                // Lets try moving horizontal
                moveSuccess = TryMove(new Vector2(moveInput.x, 0));

                // We still couldn't move..
                if (!moveSuccess)
                {
                    // Lets try moving vertical
                    TryMove(new Vector2(0, moveInput.y));
                }
            }
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