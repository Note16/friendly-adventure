using Assets.Scripts.Characters.Player;
using Assets.Scripts.Characters.Player.Attacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemies
{
    public class SpellBook : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject> spells;

        private SpriteRenderer spriteRenderer;
        private PlayerController playerController;
        public int spellSizeMultiplier = 1;
        public int spellDamageMultiplier = 1;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            playerController = FindObjectOfType<PlayerController>();
        }

        public List<Action> GetSpells()
        {
            return spells.Select(spell =>
            {
                var castingTime = 0.4f;
                Action spellAction = () => CastSpell(() =>
                {
                    var isTargetSpell = spell.GetComponent<TargetSpell>() != null;

                    var spellObject = Instantiate(spell, isTargetSpell ? playerController.transform.position : spriteRenderer.bounds.center, Quaternion.identity);

                    if (spellObject.TryGetComponent<BaseSpell>(out var baseSpell))
                    {
                        baseSpell.transform.localScale *= spellSizeMultiplier;
                        baseSpell.damage *= spellDamageMultiplier;
                    }
                }, castingTime);
                return spellAction;
            }).ToList();
        }

        private void CastSpell(Action action, float castingTime)
        {
            StartCoroutine(CastAfterDelay(castingTime, () =>
            {
                action();
            }));
        }

        IEnumerator CastAfterDelay(float delay, Action cast)
        {
            yield return new WaitForSeconds(delay);
            cast();
        }
    }
}