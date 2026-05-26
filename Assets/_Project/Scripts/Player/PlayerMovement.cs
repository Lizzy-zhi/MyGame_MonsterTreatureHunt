using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;          // ����ƶ��ٶ�
    [SerializeField] private float acceleration = 20f;       // ���ٿ�����ԽСԽ���أ�
    [SerializeField] private float deceleration = 15f;       // ���ٿ�����ԽС����ԽԶ��
    [SerializeField] private float jumpForce = 12f;          // ��Ծ����

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

        // 2. 跳跃检测
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                Debug.Log("Cannot jump: Not grounded!");
            }
        }
    }

    void FixedUpdate()
    {
        // 3. ������
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        Debug.Log("FixedUpdateִ��.�Ƿ��ڵ���: " + isGrounded);
        // 4. ���ģ�ƽ���ƶ�����׾����Դ��
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;

        // �����Ƿ��ڼ��ٻ���٣�ѡ��ͬ�ļ��ٶ�
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // ������������Ӧ��
        float movementForce = speedDifference * accelRate;
        rb.AddForce(movementForce * Vector2.right, ForceMode2D.Force);

        // 5. ��������ٶȣ���ֹ���޼��٣�
        if (Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpeed, rb.velocity.y);
        }
    }

    // �� Scene ��ͼ����ʾ�����ⷶΧ�������ã�
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }
}
