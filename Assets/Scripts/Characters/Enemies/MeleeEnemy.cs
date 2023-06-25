using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class MeleeEnemy : BaseEnemy
    {
        [SerializeField]
        private int Attack1Damage = 2;

        [SerializeField]
        private int Attack2Damage = 4;

        [SerializeField]
        private float meleeAttackRange = 4f;

        public override void TryAttack()
        {

            if (!AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2" }))
            {
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    var attack = RandomHelper.GetRandom(40) ? "Attack2" : "Attack1";
                    animator.Play(attack);
                }
            }
        }

        protected override void AnimationEvent(string command)
        {
            base.AnimationEvent(command);

            if (command == "Attack1")
            {
                // Check if still in melee range
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    playerController.TakeDamage(Attack1Damage);
                }
            }
            if (command == "Attack2")
            {
                // Check if still in melee range
                if (Vector3.Distance(playerController.transform.position, transform.position) < meleeAttackRange)
                {
                    playerController.TakeDamage(Attack2Damage);
                }
            }
        }
    }
}