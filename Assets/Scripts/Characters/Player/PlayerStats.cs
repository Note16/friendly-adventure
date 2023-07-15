using Assets.Scripts.Helpers;
using Assets.Scripts.Sounds;
using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        public bool invincible = false;

        [SerializeField]
        private HealthGlobe HealthGlobe;

        [SerializeField]
        private int healthPoints = 100;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private PlayerMovement playerMovement;
        private PlayerController playerController;
        private int currentHP;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            playerController = GetComponent<PlayerController>();
            currentHP = healthPoints;
        }

        public void ResetPlayer()
        {
            animator.Play("Idle");
            currentHP = healthPoints;
            invincible = false;
            playerMovement.canMove = true;
            playerController.canAttack = true;
            HealthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
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
            if (invincible)
                return;

            var isCrit = RandomHelper.GetRandom(50);
            if (isCrit)
                damage *= 2;

            currentHP -= damage;

            CombatTextPopup.Damage(transform, 1.4f, damage, isCrit);

            if (currentHP <= 0)
            {
                animator.Play("Death");
                StartCoroutine(OnDeathAfterDelay(1.2f));
            }
            else
                animator.Play("TakeHit");

            HealthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
        }


        IEnumerator OnDeathAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            FindAnyObjectByType<Canvas>().GetComponentInChildren<DeathPopup>(true).gameObject.SetActive(true);
            SoundManager.instance.musicSource.Stop();
            invincible = true;
            playerMovement.canMove = false;
            playerController.canAttack = false;
            spriteRenderer.enabled = false;
        }
    }
}