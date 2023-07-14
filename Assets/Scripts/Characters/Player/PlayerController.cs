using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        public GameObject AoeAttack, SwordSwingAttack;

        [SerializeField]
        private InputActionReference Movement;

        private PlayerMovement playerMovement;
        private Animator animator;

        private GameObject activeAoeAttack;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            var value = Movement.action.ReadValue<Vector2>();
            playerMovement.Move(value);
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
            {
                SwordSwingAttack.SetActive(true);
            }
        }

        // Function gets executed by Player Input
        void OnSecondaryAttack()
        {
            if (activeAoeAttack == null)
            {
                animator.Play("Cast");
                activeAoeAttack = Object.Instantiate(AoeAttack, transform.position, Quaternion.identity);
            }
        }

        // Function get executed by Player Input
        void OnAimStick(InputValue value)
        {
            var pointerInput = value.Get<Vector2>();
            var rotation = GetRotation(pointerInput);
            AimLogic(rotation);
        }

        // Function get executed by Player Input
        void OnAimMouse(InputValue value)
        {
            var pointerInput = value.Get<Vector2>();
            var rotation = GetMouseRotation(pointerInput);
            AimLogic(rotation);
        }

        // Function gets executed by Animations
        public void AnimationEvent(string command)
        {
            if (command == "StopMovement")
                playerMovement.Stop(true);
            if (command == "ContinueMovement")
                playerMovement.Stop(false);
        }

        private void AimLogic(Quaternion rotation)
        {
            var hitMarker = transform.Find("Hit Marker");
            if (hitMarker != null)
                hitMarker.rotation = rotation;

            if (SwordSwingAttack != null)
            {
                SwordSwingAttack.transform.rotation = rotation;
                SwordSwingAttack.GetComponent<SpriteRenderer>().sortingOrder = (rotation.z < 0) ? 3 : 0;
            }
        }

        private Quaternion GetRotation(Vector2 position)
        {
            var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private Quaternion GetMouseRotation(Vector2 position)
        {
            var pointerPosition = Camera.main.ScreenToWorldPoint(position);
            var relativePos = (Vector2)pointerPosition - (Vector2)transform.position;
            return GetRotation(relativePos);
        }
    }
}