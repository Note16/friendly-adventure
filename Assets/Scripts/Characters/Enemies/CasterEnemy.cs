using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    [RequireComponent(typeof(SpellBook))]
    public class CasterEnemy : BaseEnemy
    {
        [SerializeField]
        private float castingAttackRange = 8f;

        private SpellBook spellBook;

        private void Awake()
        {
            spellBook = GetComponent<SpellBook>();
        }

        public override void TryAttack()
        {
            if (AnimationIsPlaying(new string[] { "TakeHit", "Death", "Cast" }))
                return;

            if (Vector3.Distance(playerController.transform.position, transform.position) > castingAttackRange)
                return;

            if (RandomHelper.GetRandom(98))
                return;

            var randomSpell = RandomHelper.GetRandom(spellBook.GetSpells());

            // Execute random spell
            animator.Play("Cast");
            randomSpell();
        }
    }
}