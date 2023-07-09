using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies.Versions
{
    [RequireComponent(typeof(SpellBook))]
    public class WarCasterEnemy : BaseEnemy
    {
        [SerializeField]
        public int Attack1Damage = 2;

        [SerializeField]
        public int Attack2Damage = 4;

        [SerializeField]
        public float meleeAttackRange = 4f;

        [SerializeField]
        private float castingAttackRange = 8f;

        private SpellBook spellBook;

        private void Awake()
        {
            spellBook = GetComponent<SpellBook>();
        }

        public override void TryAttack()
        {
            if (AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2", "Cast" }))
                return;

            TryCasting();
            TryMeleeAttack();
        }

        public void TryCasting()
        {
            if (Vector3.Distance(playerStats.transform.position, transform.position) > castingAttackRange)
                return;

            if (RandomHelper.GetRandom(98))
                return;

            animator.Play("Cast");

            var randomSpell = RandomHelper.GetRandom(spellBook.GetSpells());

            // Execute random spell
            animator.Play("Cast");
            randomSpell();
        }

        public void TryMeleeAttack()
        {
            if (Vector3.Distance(playerStats.transform.position, transform.position) > meleeAttackRange)
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
                if (Vector3.Distance(playerStats.transform.position, transform.position) < meleeAttackRange)
                {
                    playerStats.TakeDamage(Attack1Damage);
                }
            }
            if (command == "Attack2")
            {
                // Check if still in melee range
                if (Vector3.Distance(playerStats.transform.position, transform.position) < meleeAttackRange)
                {
                    playerStats.TakeDamage(Attack2Damage);
                }
            }
        }
    }
}