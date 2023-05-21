using Assets.Scripts.Characters.Enemies;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        public GameObject AoeAttack;

        [SerializeField]
        public GameObject HitMarker;

        [SerializeField]
        private float moveSpeed = 7f;

        [SerializeField]
        private InputActionReference Movement, Attack, pointerPosition;

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
        void OnAttack()
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
            HitMarker.transform.rotation = rotation;
        }

        public void Damage(int damage)
        {
            healthPoints -= damage;

            if (healthPoints <= 0)
                Debug.Log("death");
            else
                animator.Play("TakeHit");
        }

        public void AggroMobs()
        {
            var aggroDistance = 10f;

            var enemyObjects = FindObjectsOfType<GameObject>()
                .Where(obj => obj.GetComponent<Enemy>());


            foreach (var obj in enemyObjects)
            {
                var enemy = obj.GetComponent<Enemy>();

                // Find enemies within 10f radius
                if (Vector3.Distance(obj.transform.position, transform.position) < aggroDistance)
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