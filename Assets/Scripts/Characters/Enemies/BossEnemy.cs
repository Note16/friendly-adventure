using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class BossEnemy : BaseEnemy
    {
        [SerializeField]
        private int Attack1Damage = 2;

        [SerializeField]
        private int Attack2Damage = 4;

        [SerializeField]
        private float meleeAttackRange = 4f;

        [SerializeField]
        private float castingAttackRange = 8f;

        [SerializeField]
        private GameObject Spell1;

        public override void TryAttack()
        {
            if (!AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2", "Cast" }))
            {
                if (Spell1 != null && Vector3.Distance(playerController.transform.position, transform.position) < castingAttackRange)
                {
                    if (RandomHelper.GetRandom(2))
                    {
                        animator.Play("Cast");
                    }
                }
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
            if (command == "Cast")
            {
                Instantiate(Spell1, playerController.transform.position, Quaternion.identity);
            }
        }
    }
}