using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private ParticleSystem dashParticles;

    [SerializeField] private Animator animator; // Reference to the Animator component

    private CharacterController controller;
    private Vector2 inputDirection;
    private Vector3 currentVelocity;
    private float dashTimer;
    private float dashCooldownTimer;
    private bool isDashing;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetInput();
        HandleDash();

        if (!isDashing)
        {
            HandleMovement();
        }
        else
        {
            HandleDashMovement();
        }

        UpdateTimers();
    }

    private void GetInput()
    {
        // Get raw input for snappy movement
        inputDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Dash input
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0)
        {
            StartDash();
        }
    }

    private void HandleMovement()
    {
        // Convert 2D input to 3D movement (ignoring Y axis)
        Vector3 targetVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpeed;

        // Smooth acceleration
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );

        // Move the character
        controller.Move(currentVelocity * Time.deltaTime);

        // Rotate to face movement direction
        if (inputDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(
                new Vector3(inputDirection.x, 0, inputDirection.y)
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        UpdateAnimations();
    }

    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                EndDash();
            }
        }
        else
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleDashMovement()
    {
        // Dash in the current facing direction
        Vector3 dashVelocity = transform.forward * dashSpeed;
        controller.Move(dashVelocity * Time.deltaTime);
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Visual feedback
        if (dashParticles != null)
        {
            dashParticles.Play();
        }
    }

    private void EndDash()
    {
        isDashing = false;
        currentVelocity = Vector3.zero;
    }

    private void UpdateTimers()
    {
        dashCooldownTimer = Mathf.Max(0, dashCooldownTimer - Time.deltaTime);
    }

    // Public accessors for animation/other systems
    public Vector3 GetMovementVelocity() => currentVelocity;
    public bool IsDashing() => isDashing;

    public void UpdateAnimations()
    {
        if (inputDirection.magnitude > 0.1f)
        {
            // Determine the angle of movement
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            // Map angle to animation states
            if (angle >= -22.5f && angle < 22.5f)
            {
                animator.Play("WalkRight");
            }
            else if (angle >= 22.5f && angle < 67.5f)
            {
                animator.Play("WalkRightUp");
            }
            else if (angle >= 67.5f && angle < 112.5f)
            {
                animator.Play("WalkUp");
            }
            else if (angle >= 112.5f && angle < 157.5f)
            {
                animator.Play("WalkLeftUp");
            }
            else if (angle >= -67.5f && angle < -22.5f)
            {
                animator.Play("WalkDownRight");
            }
            else if (angle >= -112.5f && angle < -67.5f)
            {
                animator.Play("WalkDown");
            }
            else if (angle >= -157.5f && angle < -112.5f)
            {
                animator.Play("WalkLeftDown");
            }
            else
            {
                animator.Play("WalkLeft");
            }
        }
        else
        {
            // Play idle animation when not moving
          //  animator.Play("Idle");
        }


    }

    //depends on us if we need it
    //jump?
    //swim?
}
