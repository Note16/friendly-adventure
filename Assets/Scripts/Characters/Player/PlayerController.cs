using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Helpers;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        public GameObject AoeAttack, SwordSwingAttack;

        [SerializeField]
        public GameObject Score;

        [SerializeField]
        private float moveSpeed = 7f;

        [SerializeField]
        private InputActionReference Movement;

        [SerializeField]
        private int healthPoints = 100;

        private int currentHP;

        private PlayerMovement playerMovement;
        private PlayerAttacks playerAttacks;

        private Animator animator;
        private HealthGlobe healthGlobe;

        private void OnEnable()
        {
            var rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            var spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = new PlayerMovement(animator, spriteRenderer, rb);
            playerMovement.SetMoveSpeed(moveSpeed);
            playerAttacks = new PlayerAttacks(animator);

            currentHP = healthPoints;
            healthGlobe = FindObjectOfType<HealthGlobe>();
        }

        private void FixedUpdate()
        {
            AggroMobs();
            AggroItems();
            playerMovement.Move(Movement.action.ReadValue<Vector2>());
        }

        // Function gets executed by Player Input
        void OnMovement(InputValue value)
        {
            var moveInput = value.Get<Vector2>();
            playerMovement.Animate(moveInput);
        }

        // Function gets executed by Player Input
        void OnMainAttack()
        {
            if (SwordSwingAttack != null)
                playerAttacks.SwordSwing(SwordSwingAttack);
        }

        // Function gets executed by Player Input
        void OnSecondaryAttack()
        {
            playerAttacks.AoeAttack(AoeAttack, transform.position);
        }

        // Function get executed by Player Input
        void OnPointerPosition(InputValue value)
        {
            var pointerInput = value.Get<Vector2>();
            var pointerPosition = Camera.main.ScreenToWorldPoint(pointerInput);

            var mouseRotation = GetMouseRotation(pointerPosition);

            var hitMarker = transform.Find("Hit Marker");
            if (hitMarker != null)
                hitMarker.rotation = mouseRotation;

            if (SwordSwingAttack != null)
            {
                SwordSwingAttack.transform.rotation = mouseRotation;
                SwordSwingAttack.GetComponent<SpriteRenderer>().sortingOrder = (mouseRotation.z < 0) ? 3 : 0;
            }
        }

        // Function gets executed by Animations
        public void AnimationEvent(string command)
        {
            if (command == "StopMovement")
                playerMovement.Stop(true);
            if (command == "ContinueMovement")
                playerMovement.Stop(false);
        }

        private Quaternion GetMouseRotation(Vector2 position)
        {
            var relativePos = position - (Vector2)transform.position;
            var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
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

            healthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
        }

        public void AggroMobs()
        {
            var aggroDistance = 10f;

            var enemyObjects = FindObjectsOfType<BaseEnemy>();
            foreach (var enemy in enemyObjects)
            {
                // Find enemies within 10f radius
                if (Vector3.Distance(enemy.transform.position, transform.position) < aggroDistance)
                {
                    enemy.Move(transform.position);
                }
                else
                {
                    enemy.StopMovement();
                }
            }
        }

        public void AggroItems()
        {
            var consumeDistance = 0.5f;
            var aggroDistance = 5f;


            var gemObjects = FindObjectsOfType<Gem>();
            foreach (var gem in gemObjects)
            {
                var playerPosition = transform.position + new Vector3(0, 1f);
                // Find items within 5f radius
                if (Vector3.Distance(gem.transform.position, playerPosition) < consumeDistance)
                {
                    Score.GetComponent<Score>().UpdateScore(gem.PickUp());
                }
                else if (Vector3.Distance(gem.transform.position, playerPosition) < aggroDistance)
                {
                    gem.Move(playerPosition);
                }
            }

            var potionObjects = FindObjectsOfType<HealthPotion>();
            foreach (var potion in potionObjects)
            {
                var playerPosition = transform.position + new Vector3(0, 1f);
                // Find items within 5f radius
                if (Vector3.Distance(potion.transform.position, playerPosition) < consumeDistance)
                {
                    var heal = potion.Consume();
                    CombatTextPopup.Heal(transform, 1.4f, heal);
                    currentHP += heal;

                    healthGlobe.UpdateHealthGlobe((float)currentHP / healthPoints);
                }
                else if (Vector3.Distance(potion.transform.position, playerPosition) < aggroDistance)
                {
                    potion.Move(playerPosition);
                }
            }
        }
    }
}