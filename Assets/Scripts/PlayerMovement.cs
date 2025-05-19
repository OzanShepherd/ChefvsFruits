using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    [Header("Jumping")]
    public float jumpForce = 5f;
    private bool isGrounded = true;

    [Header("Dashing")]
    public float dashForce = 8f;
    public float dashCooldown = 1f;
    private bool canDash = true;

    private Rigidbody rb;
    private PlayerInputActions input;
    private InputAction moveAction, jumpAction, dashAction, lockDirectionAction;

    private Vector2 moveInput;
    private bool jumpPressed = false;
    private bool dashPressed = false;
    private bool lockPressedThisFrame = false;

    private bool isLockingDirection = false;
    private Vector3 lockedForwardDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        input = new PlayerInputActions();
        input.Player.Enable();

        moveAction = input.Player.Move;
        jumpAction = input.Player.Jump;
        dashAction = input.Player.Dash;
        lockDirectionAction = input.Player.LockDirection;
        lockDirectionAction.Enable();
    }

    private void Update()
    {
        // Read inputs
        moveInput = moveAction.ReadValue<Vector2>();
        jumpPressed = jumpAction.triggered;
        dashPressed = dashAction.triggered;
        lockPressedThisFrame = lockDirectionAction.triggered;

        // Right-click lock mode
        isLockingDirection = lockDirectionAction.IsPressed();

        if (lockPressedThisFrame)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            transform.rotation = Quaternion.LookRotation(camForward);
            lockedForwardDirection = camForward;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDir;

        if (isLockingDirection)
        {
            Vector3 lockedRight = Vector3.Cross(Vector3.up, lockedForwardDirection);
            moveDir = lockedForwardDirection * moveInput.y + lockedRight * moveInput.x;
        }
        else
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * moveInput.y + camRight * moveInput.x;

            if (moveDir.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 720f * Time.deltaTime);
            }
        }

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        // Jump
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpPressed = false;
        }

        // Dash
        if (dashPressed && canDash)
        {
            Vector3 dashDirection;

            if(moveInput != Vector2.zero)
            {
                if(isLockingDirection)
                {
                    Vector3 lockedRight = Vector3.Cross(Vector3.up, lockedForwardDirection);
                    dashDirection = lockedForwardDirection * moveInput.y + lockedRight * moveInput.x;
                }
                else
                {
                    Vector3 camForward = cameraTransform.forward;
                    Vector3 camRight = cameraTransform.right;

                    camForward.y = 0f;
                    camRight.y = 0f;
                    camForward.Normalize();
                    camRight.Normalize();

                    dashDirection = camForward * moveInput.y + camRight * moveInput.x;
                }
            }
            else
            {
                dashDirection = isLockingDirection ? lockedForwardDirection : cameraTransform.forward;
            }
            dashDirection.y = 0f;
            dashDirection.Normalize();

            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
            StartCoroutine(DashCooldownRoutine());
            dashPressed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Simple ground check
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    private System.Collections.IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
