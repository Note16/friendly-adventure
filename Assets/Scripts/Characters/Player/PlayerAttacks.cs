using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerAttacks
    {
        private GameObject activeAoeAttack;
        private GameObject activeSwordSwingAttack;
        private readonly Animator animator;

        public PlayerAttacks(Animator animator)
        {
            this.animator = animator;
        }

        public void AoeAttack(GameObject aoeAttack, Vector3 position)
        {
            if (activeAoeAttack == null)
            {
                animator.Play("Cast");
                activeAoeAttack = Object.Instantiate(aoeAttack, position, Quaternion.identity);
            }
        }

        public void SwordSwing(GameObject swordSwing, Transform playerPosition)
        {
            if (activeSwordSwingAttack != null)
                GameObject.Destroy(activeSwordSwingAttack);

            var spriteRenderer = swordSwing.GetComponent<SpriteRenderer>();
            var collider2D = swordSwing.GetComponent<CapsuleCollider2D>();
            spriteRenderer.sortingOrder = 0;
            collider2D.offset = new Vector2(0.45f, 0);

            if (animator.GetFloat("moveY") < 0)
            {
                swordSwing.transform.position = new Vector3(0, 0.2f);
                swordSwing.transform.rotation = Quaternion.Euler(0, 0, 270);
                spriteRenderer.sortingOrder = 2;
                collider2D.offset = new Vector2(1f, 0);
            }
            else if (animator.GetFloat("moveY") > 0)
            {
                swordSwing.transform.position = new Vector3(0, 0.7f);
                swordSwing.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (animator.GetFloat("moveX") < 0)
            {
                swordSwing.transform.rotation = new Quaternion(0, 0, 180f, 0);
                swordSwing.transform.position = new Vector3(-0.5f, 0.5f);
            }
            else
            {
                swordSwing.transform.rotation = new Quaternion(0, 0, 0, 0);
                swordSwing.transform.position = new Vector3(0.5f, 0.5f);
            }

            activeSwordSwingAttack = Object.Instantiate(swordSwing, playerPosition);
        }
    }
}