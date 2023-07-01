using Assets.Scripts.Helpers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        private HealthGlobe HealthGlobe;

        [SerializeField]
        private int healthPoints = 100;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private int currentHP;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            currentHP = healthPoints;
        }

        public Vector3 GetSpriteCenter()
        {
            return spriteRenderer.bounds.center;
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            HealthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
        }

        public void TakeDamage(int damage)
        {
            var isCrit = RandomHelper.GetRandom(50);
            if (isCrit)
                damage *= 2;

            currentHP -= damage;

            CombatTextPopup.Damage(transform, 1.4f, damage, isCrit);

            if (currentHP <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");

            HealthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
        }
    }
}