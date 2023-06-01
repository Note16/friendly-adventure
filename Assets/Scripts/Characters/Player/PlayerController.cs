using Assets.Scripts.Characters.Enemies;
using Assets.Scripts.Helpers;
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
        private float moveSpeed = 7f;

        [SerializeField]
        private InputActionReference Movement;

        [SerializeField]
        private int healthPoints = 100;

        private PlayerMovement playerMovement;
        private PlayerAttacks playerAttacks;

        private Animator animator;

        private void OnEnable()
        {
            var rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            var spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = new PlayerMovement(animator, spriteRenderer, rb);
            playerMovement.SetMoveSpeed(moveSpeed);
            playerAttacks = new PlayerAttacks(animator);
        }

        private void FixedUpdate()
        {
            AggroMobs();
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
            playerAttacks.SwordSwing(SwordSwingAttack, transform);
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

            RotateHitMarker(pointerPosition);
        }

        // Function gets executed by Animations
        public void AnimationEvent(string command)
        {
            if (command == "StopMovement")
                playerMovement.Stop(true);
            if (command == "ContinueMovement")
                playerMovement.Stop(false);
        }

        private void RotateHitMarker(Vector2 position)
        {
            var relativePos = position - (Vector2)transform.position;
            var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var hitMarker = transform.Find("Hit Marker");
            if (hitMarker != null)
                hitMarker.rotation = rotation;
        }

        public void Damage(int damage)
        {
            var isCrit = RandomHelper.GetRandom(50);
            if (isCrit)
                damage *= 2;

            healthPoints -= damage;

            DamagePopup.Create(transform, 1.4f, damage, isCrit);

            if (healthPoints <= 0)
                animator.Play("Death");
            else
                animator.Play("TakeHit");
        }

        public void AggroMobs()
        {
            var aggroDistance = 10f;

            var enemyObjects = FindObjectsOfType<Enemy>();
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
    }
}