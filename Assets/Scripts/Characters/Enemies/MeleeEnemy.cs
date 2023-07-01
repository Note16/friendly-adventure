using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class MeleeEnemy : BaseEnemy
    {
        [SerializeField]
        public int Attack1Damage = 2;

        [SerializeField]
        public int Attack2Damage = 4;

        [SerializeField]
        public float meleeAttackRange = 4f;

        public override void TryAttack()
        {
            if (AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2" }))
                return;

            if (Vector3.Distance(playerController.transform.position, transform.position) > meleeAttackRange)
                return;

            var attack = RandomHelper.GetRandom(40) ? "Attack2" : "Attack1";
            animator.Play(attack);
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