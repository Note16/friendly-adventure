using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject AoeAttack;

    [SerializeField]
    private InputActionReference Movement, Attack, pointerPosition;

    public Vector2 PointerPosition { get; set; }
    private Vector2 pointerInput;

    public float moveSpeed = 5f;
    public ContactFilter2D moveFilter;
    public float collisionOffset = 0.05f;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;
    private GameObject isAttacking;
    Vector2 moveInput;


    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            moveInput = Movement.action.ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                bool success = TryMove(moveInput);

                if (!success)
                {
                    success = TryMove(new Vector2(moveInput.x, 0));

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, moveInput.y));
                    }
                }

                if (moveInput.x < 0)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;

                animator.SetBool("isMovingHorizontal", moveInput.x != 0);
                animator.SetBool("isMovingVertical", moveInput.y != 0);
                animator.SetBool("isMoving", true);

                animator.SetFloat("moveX", moveInput.x);
                animator.SetFloat("moveY", moveInput.y);
            }
            else
            {
                animator.SetBool("isMovingHorizontal", false);
                animator.SetBool("isMovingVertical", false);
                animator.SetBool("isMoving", false);
            }
        }
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
        //     if (lookDirection.y > 3)
        //     {
        //         animator.Play("LookUp");
        //     }
        //     else if (lookDirection.y < 0.2)
        //     {
        //         animator.Play("LookDown");
        //     }
        // }

    }

    void OnAttack()
    {
        // Melee attack
        animator.Play("Attack_1");

        // If not attacking
        if (isAttacking == null)
        {
            // AOE Attack!
            isAttacking = Instantiate(AoeAttack, gameObject.transform.position, Quaternion.identity);
        }

    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for collision
            int count = rb.Cast(
                direction,
                moveFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
        }
        return false;
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
