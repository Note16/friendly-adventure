using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerAttacks
    {
        private GameObject activeAoeAttack;
        private readonly Animator animator;

        public PlayerAttacks(Animator animator)
        {
            this.animator = animator;
        }

        public void AoeAttack(GameObject AoeAttack, Vector3 position)
        {
            // If not attacking
            if (activeAoeAttack == null)
            {
                // Melee attack
                animator.Play("Attack_1");


                // AOE Attack!
                activeAoeAttack = Object.Instantiate(AoeAttack, position, Quaternion.identity);
            }
        }
    }
}