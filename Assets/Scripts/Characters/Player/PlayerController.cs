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
    }
}