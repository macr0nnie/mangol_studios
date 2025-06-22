using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;

    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Vector2 inputDirection;
    private Vector3 currentVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetInput();
        HandleMovement();
        UpdateAnimations();
    }

    private void GetInput()
    {
        // WASD or Arrow keys input
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down

        // No isometric rotation here; we convert to isometric in movement
        inputDirection = new Vector2(vertical, -horizontal).normalized;
    }

    private void HandleMovement()
    {
        // Convert 2D input to isometric 3D movement
        // Isometric right: (1, 0, 1), Isometric up: (-1, 0, 1)
        Vector3 isoRight = new Vector3(1, 0, 1).normalized;
        Vector3 isoUp = new Vector3(-1, 0, 1).normalized;

        Vector3 targetVelocity = (isoRight * inputDirection.x + isoUp * inputDirection.y) * moveSpeed;

        // Smooth acceleration
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );

        controller.Move(currentVelocity * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // For blend tree: set parameters instead of playing states directly
        animator.SetFloat("MoveX", inputDirection.x);
        animator.SetFloat("MoveY", inputDirection.y);
        animator.SetBool("IsMoving", inputDirection.magnitude > 0.1f);
    }
}