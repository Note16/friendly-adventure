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
        private float moveSpeed = 7f;

        [SerializeField]
        private InputActionReference Movement, Attack, pointerPosition;

        private PlayerMovement playerMovement;
        private PlayerAttacks playerAttacks;

        private void OnEnable()
        {
            var rb = GetComponent<Rigidbody2D>();
            var animator = GetComponent<Animator>();
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

        // Function gets executed by Animations
        public void AnimationEvent(string command)
        {
            if (command == "StopMovement")
                playerMovement.Stop(true);
            if (command == "ContinueMovement")
                playerMovement.Stop(false);
        }

        public void AggroMobs()
        {
            var distance = 10f;
            var enemies = FindObjectsOfType<GameObject>()
                .Where(obj =>
                    obj.GetComponent<Enemy>() &&
                    Vector3.Distance(obj.transform.position, transform.position) < distance
                ).Select(obj => obj.GetComponent<Enemy>());

            if (!enemies.Any())
                return;

            foreach (var enemy in enemies)
            {
                enemy.Move(transform.position);
            }
        }

        /* TODO: Player AIM controls
         * 
        public Vector2 PointerPosition { get; set; }
        private Vector2 pointerInput;
        private Vector2 GetPointerInput()
        {
            Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
            mousePos.z = Camera.main.nearClipPlane;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
        public void PlayerAim()
        {
            //Look at cursor when not moving
            // pointerInput = GetPointerInput();
            // var lookDirection = pointerInput - (Vector2)transform.position;
            // if (isMoving == false)
            // {
            //     if (lookDirection.x > 0.2)
            //     {
            //         spriteRenderer.flipX = false;
            //     }
            //     else if (lookDirection.x < 0.2)
            //     {
            //         spriteRenderer.flipX = true;
            //     }
            // }
        }
        */
    }
}