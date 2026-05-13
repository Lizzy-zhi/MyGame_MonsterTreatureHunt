using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;          // 最大移动速度
    [SerializeField] private float acceleration = 20f;       // 加速快慢（越小越笨重）
    [SerializeField] private float deceleration = 15f;       // 减速快慢（越小滑行越远）
    [SerializeField] private float jumpForce = 12f;          // 跳跃力度

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. 获取输入
        horizontalInput = Input.GetAxis("Horizontal");

        // 2. 跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // 3. 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // 4. 核心：平滑移动（笨拙感来源）
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;

        // 根据是否在加速或减速，选择不同的加速度
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // 计算最终力并应用
        float movementForce = speedDifference * accelRate;
        rb.AddForce(movementForce * Vector2.right, ForceMode2D.Force);

        // 5. 限制最大速度（防止无限加速）
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);
        }
    }

    // 在 Scene 视图中显示地面检测范围（调试用）
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
