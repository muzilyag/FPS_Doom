using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Speed")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 4f;
    private float currentSpeed;

    [Header("Jump")]
    public float gravity = -9.81f * 2;
    public float jumpHeight = 1.2f;
    private bool isJumping;

    [Header("Crouch")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    private float targetHeight;
    private bool isCrouching;

    [Header("Ground—heck")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private RaycastHit groundHit;

    private Vector3 velocity;
    private Vector3 lastPosition;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        standingHeight = controller.height;
        targetHeight = standingHeight;
        currentSpeed = walkSpeed;
        UpdateGroundCheckPosition();
    }

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        ApplyGravity();
    }

    private void HandleGroundCheck()
    {
        bool wasGrounded = isGrounded;

        isGrounded = Physics.SphereCast(
            groundCheck.position,
            groundCheckRadius,
            Vector3.down,
            out groundHit,
            groundCheckDistance,
            groundMask);

        if (!isGrounded)
        {
            isGrounded = controller.isGrounded;
        }

        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
        }

        Debug.DrawRay(groundCheck.position,
                     Vector3.down * (groundCheckDistance + groundCheckRadius),
                     isGrounded ? Color.green : Color.red);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void UpdateGroundCheckPosition()
    {
        if (groundCheck != null)
        {
            groundCheck.position = transform.position + Vector3.down * (controller.height / 2 - 0.1f);
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float targetSpeed = isCrouching ? crouchSpeed :
                          Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 10f);

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && !isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                if (!Physics.Raycast(transform.position, Vector3.up, standingHeight - crouchingHeight + 0.1f))
                {
                    isCrouching = false;
                    targetHeight = standingHeight;
                }
            }
            else
            {
                isCrouching = true;
                targetHeight = crouchingHeight;
            }
        }

        if (Mathf.Abs(controller.height - targetHeight) > 0.01f)
        {
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
            UpdateGroundCheckPosition();
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position + Vector3.down * groundCheckDistance, groundCheckRadius);
        }
    }
}