using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class CasterEnemy : BaseEnemy
    {
        [SerializeField]
        private float castingAttackRange = 8f;

        [SerializeField]
        private GameObject TargetSpell;

        [SerializeField]
        private GameObject ProjectileSpell;

        public override void TryAttack()
        {
            if (AnimationIsPlaying(new string[] { "TakeHit", "Death", "Attack1", "Attack2", "Cast" }))
                return;

            if (Vector3.Distance(playerController.transform.position, transform.position) > castingAttackRange)
                return;

            if (RandomHelper.GetRandom(98))
                return;

            var randomSpell = RandomHelper.GetRandom(GetSpells());

            // Execute random spell
            randomSpell();
        }

        private List<Action> GetSpells()
        {
            var spells = new List<Action>();

            if (ProjectileSpell != null)
            {
                spells.Add(() => CastSpell(() => Instantiate(ProjectileSpell, transform.position - new Vector3(0, -1, 0), Quaternion.identity)));
            };
            if (TargetSpell != null)
            {
                spells.Add(() => CastSpell(() => Instantiate(TargetSpell, playerController.transform.position, Quaternion.identity)));
            }

            return spells;
        }

        private void CastSpell(Action cast)
        {
            animator.Play("Cast");
            StartCoroutine(CastAfterDelay(0.4f, () =>
            {
                cast();
            }));
        }

        IEnumerator CastAfterDelay(float delay, Action cast)
        {
            yield return new WaitForSeconds(delay);
            cast();
        }
    }
}