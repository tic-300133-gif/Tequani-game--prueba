using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float jumpForce = 7f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    public bool isGrounded;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private float horizontalInput;
    private bool canJump = true;
    private float jumpCooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 1. Detección estricta de suelo excluyendo al propio jugador
        isGrounded = CheckGround();

        // Si tocó suelo real y pasó el tiempo de seguridad, permite saltar de nuevo
        if (isGrounded && Time.time >= jumpCooldownTimer)
        {
            canJump = true;
        }

        // 2. Entrada Horizontal
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        // 3. Salto único
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
            isGrounded = false;
            jumpCooldownTimer = Time.time + 0.15f;
        }

        // Control de salto corto al soltar la tecla
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // 4. Actualizar Animator de Movimiento
        anim.SetFloat("mover", Mathf.Abs(horizontalInput));
        anim.SetBool("IsJumping", !isGrounded);

        // 5. ATAQUE (Integrado aquí - Control izquierdo o Clic izquierdo)
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Golpear");
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private bool CheckGround()
    {
        if (groundCheck == null) return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);

        foreach (var col in colliders)
        {
            if (col != null && col != playerCollider && !col.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}