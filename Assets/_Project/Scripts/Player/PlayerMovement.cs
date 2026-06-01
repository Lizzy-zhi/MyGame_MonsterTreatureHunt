using UnityEngine;

namespace MonsterTreasureHunt.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Clumsy Movement")]
        [SerializeField] private float maxRunSpeed = 6.2f;
        [SerializeField] private float groundAcceleration = 14f;
        [SerializeField] private float groundDeceleration = 6f;
        [SerializeField] private float turnResponsiveness = 0.35f;
        [SerializeField] private float skidDrag = 0.9f;

        [Header("Air Control")]
        [SerializeField] private float airAccelerationMultiplier = 0.55f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private float fallGravityMultiplier = 2f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D rb;
        private float horizontalInput;
        private bool jumpQueued;
        private bool isGrounded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpQueued = true;
            }
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

            HandleHorizontalMovement();
            HandleJump();
            ApplyBetterFall();
        }

        private void HandleHorizontalMovement()
        {
            float targetSpeed = horizontalInput * maxRunSpeed;
            float currentSpeed = rb.velocity.x;

            float accel = horizontalInput == 0f ? groundDeceleration : groundAcceleration;
            if (!isGrounded)
            {
                accel *= airAccelerationMultiplier;
            }

            bool reversing = Mathf.Abs(horizontalInput) > 0.01f && Mathf.Sign(horizontalInput) != Mathf.Sign(currentSpeed) && Mathf.Abs(currentSpeed) > 0.25f;
            if (reversing)
            {
                targetSpeed = Mathf.Lerp(currentSpeed, targetSpeed, turnResponsiveness);
            }

            float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);

            if (isGrounded && Mathf.Abs(horizontalInput) < 0.01f)
            {
                newSpeed *= skidDrag;
            }

            rb.velocity = new Vector2(newSpeed, rb.velocity.y);
        }

        private void HandleJump()
        {
            if (!jumpQueued) return;
            jumpQueued = false;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        private void ApplyBetterFall()
        {
            if (rb.velocity.y < 0f)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1f) * Time.fixedDeltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
