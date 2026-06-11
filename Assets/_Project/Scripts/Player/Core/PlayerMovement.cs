using MonsterTreasureHunt.Gameplay;
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

        [Header("Ladder")]
        [SerializeField] private float ladderClimbSpeed = 4.1f;
        [SerializeField] private float ladderHorizontalSnapSpeed = 7.5f;
        [SerializeField] private float ladderTopExitTolerance = 0.12f;
        [SerializeField] private float ladderTopExitGroundClearance = 0.03f;

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
        [SerializeField] private Sprite climbSpriteA;
        [SerializeField] private Sprite climbSpriteB;
        [SerializeField] private float runAnimationRate = 9f;

        [Header("Hurt Feedback")]
        [SerializeField] private Color hurtTint = new Color(1f, 0.42f, 0.42f, 1f);
        [SerializeField] private float hurtFlashDuration = 0.45f;
        [SerializeField] private float hurtKnockbackX = 4.5f;
        [SerializeField] private float hurtKnockbackY = 6f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D bodyCollider;
        private float horizontalInput;
        private float verticalInput;
        private bool jumpQueued;
        private bool isGrounded;
        private bool isCrouching;
        private bool isClimbingLadder;
        private float runAnimationTimer;
        private Vector2 standingColliderSize;
        private Vector2 standingColliderOffset;
        private float hurtFlashUntil;
        private float defaultGravityScale;
        private LadderZone currentLadderZone;
        private LadderZone ladderBlockedUntilInputRelease;

        public bool IsGrounded => isGrounded;
        public bool IsHurt => Time.time < hurtFlashUntil;
        public bool IsCrouching => isCrouching;
        public bool IsClimbingLadder => isClimbingLadder;
        public Rigidbody2D Body => rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            bodyCollider = GetComponent<BoxCollider2D>();
            defaultGravityScale = rb != null ? rb.gravityScale : 1f;

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
            verticalInput = Input.GetAxisRaw("Vertical");

            UpdateLadderState();

            if (isClimbingLadder)
            {
                isCrouching = false;
                jumpQueued = false;
            }
            else
            {
                bool crouchRequested = IsCrouchPressed();
                if (crouchRequested)
                {
                    isCrouching = true;
                }
                else if (isCrouching && CanStandUp())
                {
                    isCrouching = false;
                }
            }

            if (isClimbingLadder)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    JumpOffLadder();
                }
            }
            else if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                jumpQueued = true;
            }

            UpdateColliderForCrouch();
            UpdateAnimation();
            UpdateHurtVisual();
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

            if (isClimbingLadder)
            {
                HandleLadderMovement();
                return;
            }

            HandleHorizontalMovement();
            HandleJump();
            ApplyBetterFall();
        }

        private void UpdateLadderState()
        {
            if (currentLadderZone == null)
            {
                ladderBlockedUntilInputRelease = null;

                if (isClimbingLadder)
                {
                    StopClimbingLadder();
                }

                return;
            }

            if (!isClimbingLadder && currentLadderZone == ladderBlockedUntilInputRelease)
            {
                if (Mathf.Abs(verticalInput) > 0.2f)
                {
                    return;
                }

                ladderBlockedUntilInputRelease = null;
            }

            bool climbRequested = Mathf.Abs(verticalInput) > 0.2f;
            if (!isClimbingLadder && climbRequested)
            {
                StartClimbingLadder();
                return;
            }

            if (!isClimbingLadder) return;

            if (isGrounded && Mathf.Abs(horizontalInput) > 0.1f)
            {
                StopClimbingLadder();
                return;
            }

            if (isGrounded && Mathf.Abs(verticalInput) < 0.1f)
            {
                StopClimbingLadder();
            }
        }

        private void StartClimbingLadder()
        {
            if (currentLadderZone == null || rb == null) return;

            isClimbingLadder = true;
            isCrouching = false;
            jumpQueued = false;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        private void StopClimbingLadder()
        {
            isClimbingLadder = false;

            if (rb != null)
            {
                rb.gravityScale = defaultGravityScale;
            }
        }

        private void HandleLadderMovement()
        {
            if (rb == null || currentLadderZone == null)
            {
                StopClimbingLadder();
                return;
            }

            Vector2 nextPosition = rb.position;
            nextPosition.x = Mathf.MoveTowards(nextPosition.x, currentLadderZone.SnapX, ladderHorizontalSnapSpeed * Time.fixedDeltaTime);
            float verticalDelta = verticalInput * ladderClimbSpeed * Time.fixedDeltaTime;

            if (verticalDelta > 0f && TryExitTopOfLadder(verticalDelta))
            {
                return;
            }

            nextPosition.y += verticalDelta;

            rb.velocity = Vector2.zero;
            rb.MovePosition(nextPosition);
        }

        private bool TryExitTopOfLadder(float verticalDelta)
        {
            if (currentLadderZone == null || !currentLadderZone.HasTopExit || bodyCollider == null || rb == null)
            {
                return false;
            }

            Bounds bounds = bodyCollider.bounds;
            float predictedTop = bounds.max.y + verticalDelta;
            if (predictedTop < currentLadderZone.TopPlatformBottomY - ladderTopExitTolerance)
            {
                return false;
            }

            float targetBottomY = currentLadderZone.TopSurfaceY + ladderTopExitGroundClearance;
            float targetCenterX = currentLadderZone.TopExitX;
            Vector2 exitPosition = rb.position;
            exitPosition.x += targetCenterX - bounds.center.x;
            exitPosition.y += targetBottomY - bounds.min.y;

            LadderZone exitedLadder = currentLadderZone;
            StopClimbingLadder();
            isCrouching = false;
            jumpQueued = false;
            ladderBlockedUntilInputRelease = exitedLadder;
            rb.velocity = Vector2.zero;
            rb.position = exitPosition;
            return true;
        }

        private void JumpOffLadder()
        {
            if (rb == null) return;

            StopClimbingLadder();
            float jumpDirection = Mathf.Abs(horizontalInput) > 0.05f ? Mathf.Sign(horizontalInput) : 0f;
            rb.velocity = new Vector2(jumpDirection * maxRunSpeed * 0.35f, jumpForce);
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
            if (currentLadderZone != null && Mathf.Abs(verticalInput) > 0.2f) return false;

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

        public void PlayHurtFeedback(float knockbackDirectionX)
        {
            hurtFlashUntil = Time.time + hurtFlashDuration;
            StopClimbingLadder();

            if (rb == null) return;

            float direction = Mathf.Abs(knockbackDirectionX) > 0.01f
                ? Mathf.Sign(knockbackDirectionX)
                : (spriteRenderer != null && spriteRenderer.flipX ? -1f : 1f);

            rb.velocity = new Vector2(direction * hurtKnockbackX, hurtKnockbackY);
        }

        private void UpdateHurtVisual()
        {
            if (spriteRenderer == null) return;

            spriteRenderer.color = IsHurt ? hurtTint : Color.white;
        }

        private void UpdateAnimation()
        {
            if (spriteRenderer == null) return;

            float visualSpeedX = rb != null ? rb.velocity.x : 0f;
            if (Mathf.Abs(visualSpeedX) > 0.05f)
            {
                spriteRenderer.flipX = visualSpeedX < 0f;
            }

            if (IsHurt)
            {
                SetSpriteIfPresent(crouchSprite != null ? crouchSprite : idleSprite);
                runAnimationTimer = 0f;
                return;
            }

            if (isClimbingLadder)
            {
                runAnimationTimer += Time.deltaTime * runAnimationRate;
                bool useA = Mathf.FloorToInt(runAnimationTimer) % 2 == 0;
                Sprite climbSprite = useA ? climbSpriteA : climbSpriteB;
                if (Mathf.Abs(verticalInput) <= 0.05f)
                {
                    climbSprite = climbSpriteA != null ? climbSpriteA : climbSpriteB;
                }

                SetSpriteIfPresent(climbSprite != null ? climbSprite : (jumpSprite != null ? jumpSprite : idleSprite));
                return;
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

        public void ApplySkin(Sprite idle, Sprite runA, Sprite runB, Sprite jump, Sprite crouch, Sprite climbA = null, Sprite climbB = null)
        {
            idleSprite = idle != null ? idle : idleSprite;
            runSpriteA = runA != null ? runA : runSpriteA;
            runSpriteB = runB != null ? runB : runSpriteB;
            jumpSprite = jump != null ? jump : jumpSprite;
            crouchSprite = crouch != null ? crouch : crouchSprite;
            climbSpriteA = climbA != null ? climbA : climbSpriteA;
            climbSpriteB = climbB != null ? climbB : climbSpriteB;

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
                SetSpriteIfPresent(idleSprite);
            }
        }

        public void SetLadderZone(LadderZone ladderZone, bool isInside)
        {
            if (isInside)
            {
                currentLadderZone = ladderZone;
                return;
            }

            if (currentLadderZone != ladderZone) return;

            currentLadderZone = null;
            if (isClimbingLadder)
            {
                StopClimbingLadder();
            }
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
