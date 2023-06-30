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

        public void SwordSwing(GameObject swordSwing)
        {
            swordSwing.SetActive(true);
        }
    }
}