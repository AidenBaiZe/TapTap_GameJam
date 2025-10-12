using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 5f;          // �ƶ��ٶ�
    [SerializeField] private float acceleration = 10f;      // ���ٶ�
    [SerializeField] private float deceleration = 8f;       // ���ٶ�
    [SerializeField] private float maxSpeed = 8f;           // ����ٶ�

    [Header("�������")]
    [SerializeField] private float friction = 0.2f;         // Ħ����
    [SerializeField] private bool usePhysics = true;        // �Ƿ�ʹ������ϵͳ

    [Header("�������")]
    [SerializeField] private Rigidbody2D rb;               // 2D��������
    [SerializeField] private Transform graphics;           // �Ӿ����֣����ڷ�ת��

    // �ڲ�����
    private float horizontalInput;
    private float currentSpeed;
    private bool isFacingRight = true;

    // ������أ���ѡ��
    private Animator animator;
    private bool hasAnimator;

    void Start()
    {
        // �Զ���ȡ��������û����Inspector�и�ֵ��
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (graphics == null)
            graphics = transform.Find("Graphics"); // �����Ӿ���������Ϊ"Graphics"���Ӷ�����

        // ��ȡ�����������ѡ��
        animator = GetComponent<Animator>();
        hasAnimator = animator != null;

        // ��ʼ���ٶ�
        currentSpeed = 0f;
    }

    void Update()
    {
        GetInput();
        HandleAnimation();
        FlipCharacter();
    }

    void FixedUpdate()
    {
        if (usePhysics && rb != null)
        {
            MoveWithPhysics();
        }
        else
        {
            MoveWithTransform();
        }
    }

    // ��ȡ�������
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // ʹ��Raw���˲ʱ����

        // ������Ϣ���ڿ���ʱ���ã�
        Debug.Log($"Input: {horizontalInput}, Current Speed: {currentSpeed}");
    }

    // ʹ������ϵͳ�ƶ�������ʵ��
    void MoveWithPhysics()
    {
        Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // ƽ�����ɵ�Ŀ���ٶ�
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Ӧ��Ħ��������û������ʱ��
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x * (1 - friction), rb.velocity.y);
        }

        // ��������ٶ�
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    // ʹ��Transform�ƶ�������ֱ�ӣ�
    void MoveWithTransform()
    {
        // ����Ŀ���ٶ�
        float targetSpeed = horizontalInput * moveSpeed;

        // ���ٺͼ���
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // ����
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // ����
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        // ��������ٶ�
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Ӧ���ƶ�
        Vector3 movement = new Vector3(currentSpeed, 0f, 0f) * Time.fixedDeltaTime;
        transform.Translate(movement, Space.World);
    }

    // ��ת��ɫ����
    void FlipCharacter()
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // ����е������Ӿ����֣���ת��
        if (graphics != null)
        {
            Vector3 scale = graphics.localScale;
            scale.x *= -1;
            graphics.localScale = scale;
        }
        else
        {
            // ���û�У���ת��������
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // ����������ѡ��
    void HandleAnimation()
    {
        if (!hasAnimator) return;

        // �����ƶ��ٶȲ���
        float speed = usePhysics ? Mathf.Abs(rb.velocity.x) : Mathf.Abs(currentSpeed);
        animator.SetFloat("Speed", speed);

        // �����Ƿ��ڵ�������������Ҫ��
        // animator.SetBool("IsGrounded", IsGrounded());
    }

    // ���������������ⲿ����
    public void SetMovementEnabled(bool enabled)
    {
        this.enabled = enabled;
        if (rb != null && !enabled)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public float GetCurrentSpeed()
    {
        return usePhysics ? rb.velocity.x : currentSpeed;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(GetCurrentSpeed()) > 0.1f;
    }
}