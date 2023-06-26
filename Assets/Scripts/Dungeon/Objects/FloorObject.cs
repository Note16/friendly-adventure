using Assets.Scripts.Characters.Player;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Objects
{
    public class FloorObject : MonoBehaviour
    {
        protected PlayerController playerController;
        protected SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerController = FindObjectOfType<PlayerController>();
        }

        private void FixedUpdate()
        {
            // Update sorting order so enemy shows on top of player
            // When player is positioned above the enemy
            var relativePos = (Vector2)playerController.transform.position - (Vector2)transform.position;

            var pivitPoint = spriteRenderer.size.y - 3f;
            spriteRenderer.sortingOrder = relativePos.y < pivitPoint ? 0 : 2;
        }

    }
}