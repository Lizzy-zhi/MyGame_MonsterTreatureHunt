using UnityEngine;

namespace MonsterTreasureHunt.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
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

        [Header("Crouch")]
        [SerializeField] private float crouchSpeedMultiplier = 0.2f;
        [SerializeField] private Vector2 crouchColliderSize = new Vector2(0.38f, 0.75f);
        [SerializeField] private Vector2 crouchColliderOffset = new Vector2(0f, 0.475f);
        [SerializeField] private float standUpHeadCheckExtraHeight = 0.06f;

        [Header("Animation Sprites")]
        [SerializeField] private Sprite idleSprite;
        [SerializeField] private Sprite runSpriteA;
        [SerializeField] private Sprite runSpriteB;
        [SerializeField] private Sprite jumpSprite;
        [SerializeField] private Sprite crouchSprite;
        [SerializeField] private float runAnimationRate = 9f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D bodyCollider;
        private float horizontalInput;
        private bool jumpQueued;
        private bool isGrounded;
        private bool isCrouching;
        private float runAnimationTimer;
        private Vector2 standingColliderSize;
        private Vector2 standingColliderOffset;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            bodyCollider = GetComponent<BoxCollider2D>();

            if (bodyCollider != null)
            {
                standingColliderSize = bodyCollider.size;
                standingColliderOffset = bodyCollider.offset;
            }
        }

        private void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
            horizontalInput = Input.GetAxisRaw("Horizontal");
            bool crouchRequested = IsCrouchPressed();
            if (crouchRequested)
            {
                isCrouching = true;
            }
            else if (isCrouching && CanStandUp())
            {
                isCrouching = false;
            }

            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                jumpQueued = true;
            }

            UpdateColliderForCrouch();
            UpdateAnimation();
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
            float speedMultiplier = isCrouching ? crouchSpeedMultiplier : 1f;
            float moveInput = horizontalInput * speedMultiplier;
            float targetSpeed = moveInput * maxRunSpeed;
            float currentSpeed = rb.velocity.x;

            bool hasInput = Mathf.Abs(horizontalInput) > 0.01f;
            float accel = hasInput ? groundAcceleration : groundDeceleration;
            if (!isGrounded)
            {
                accel *= airAccelerationMultiplier;
            }

            bool reversing = hasInput &&
                             Mathf.Sign(horizontalInput) != Mathf.Sign(currentSpeed) &&
                             Mathf.Abs(currentSpeed) > 0.25f;
            if (reversing)
            {
                targetSpeed = Mathf.Lerp(currentSpeed, targetSpeed, turnResponsiveness);
            }

            float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);

            if (isGrounded && !hasInput)
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

        private bool IsCrouchPressed()
        {
            return Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.DownArrow) ||
                   Input.GetAxisRaw("Vertical") < -0.25f;
        }

        private void UpdateColliderForCrouch()
        {
            if (bodyCollider == null) return;

            bodyCollider.size = isCrouching ? crouchColliderSize : standingColliderSize;
            bodyCollider.offset = isCrouching ? crouchColliderOffset : standingColliderOffset;
        }

        private bool CanStandUp()
        {
            if (bodyCollider == null || !isGrounded) return true;

            float standHeight = standingColliderSize.y;
            float crouchHeight = crouchColliderSize.y;
            float checkHeight = standHeight - crouchHeight + standUpHeadCheckExtraHeight;
            if (checkHeight <= 0f) return true;

            Bounds bounds = bodyCollider.bounds;
            Vector2 checkCenter = new Vector2(bounds.center.x, bounds.max.y + checkHeight * 0.5f);
            Vector2 checkSize = new Vector2(bounds.size.x * 0.92f, checkHeight);

            Collider2D blocker = Physics2D.OverlapBox(checkCenter, checkSize, 0f, groundLayer);
            return blocker == null;
        }

        private void UpdateAnimation()
        {
            if (spriteRenderer == null) return;

            float visualSpeedX = rb != null ? rb.velocity.x : 0f;
            if (Mathf.Abs(visualSpeedX) > 0.05f)
            {
                spriteRenderer.flipX = visualSpeedX < 0f;
            }

            if (!isGrounded)
            {
                SetSpriteIfPresent(jumpSprite);
                runAnimationTimer = 0f;
                return;
            }

            if (isCrouching)
            {
                SetSpriteIfPresent(crouchSprite);
                runAnimationTimer = 0f;
                return;
            }

            if (Mathf.Abs(visualSpeedX) > 0.15f)
            {
                runAnimationTimer += Time.deltaTime * runAnimationRate;
                bool useA = Mathf.FloorToInt(runAnimationTimer) % 2 == 0;
                SetSpriteIfPresent(useA ? runSpriteA : runSpriteB);
                return;
            }

            runAnimationTimer = 0f;
            SetSpriteIfPresent(idleSprite);
        }

        private void SetSpriteIfPresent(Sprite sprite)
        {
            if (sprite == null) return;
            spriteRenderer.sprite = sprite;
        }

        public void SetBodyTint(Color tint)
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = tint;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);

            if (bodyCollider == null || !isCrouching) return;
            Gizmos.color = Color.cyan;
            Bounds bounds = bodyCollider.bounds;
            float standHeight = standingColliderSize.y;
            float crouchHeight = crouchColliderSize.y;
            float checkHeight = standHeight - crouchHeight + standUpHeadCheckExtraHeight;
            if (checkHeight <= 0f) return;
            Vector3 checkCenter = new Vector3(bounds.center.x, bounds.max.y + checkHeight * 0.5f, 0f);
            Vector3 checkSize = new Vector3(bounds.size.x * 0.92f, checkHeight, 0f);
            Gizmos.DrawWireCube(checkCenter, checkSize);
        }
    }
}
