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
            // Update sorting order so objects show on top of player
            // When player is positioned above the object
            var relativePos = (Vector2)playerController.transform.position - (Vector2)transform.position;

            var pivitPoint = spriteRenderer.size.y - 1f;
            spriteRenderer.sortingOrder = relativePos.y < pivitPoint ? 0 : 2;
        }

    }
}