using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundAndPlatformLayer;
    private bool isGrounded;
    private bool wasGrounded;

    [Header("Input System")]
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference attack;

    private Vector2 moveDirection;

    [Header("Paso")]
    public PasoController paso;
    public float distanciaPaso = 2f;

    private Collider2D playerCollider;

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        jump.action.performed += OnJump;
        attack.action.Enable();
        attack.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        jump.action.performed -= OnJump;
        attack.action.Disable();
        attack.action.performed -= OnAttack;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();

        // Flip del personaje
        if (moveDirection.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveDirection.x < -0.1f)
            spriteRenderer.flipX = true;

        // Animación de caminar
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x));

        // Control de caída y salto (gravedad modificada)
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jump.action.IsPressed())
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Detección de suelo
        int combinedGroundMask;
        combinedGroundMask = LayerMask.GetMask("Ground", "Platform");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, combinedGroundMask);

        if (!wasGrounded && isGrounded)
        {
            // Aterrizó
            animator.SetBool("IsJumping", false);
        }
        else if (wasGrounded && !isGrounded)
        {
            // Despegó
            animator.SetBool("IsJumping", true);
        }
        wasGrounded = isGrounded;

        // Salto desde Input System
        if (isGrounded && jump.action.WasPerformedThisFrame())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

        // Detecta si está de frente y cerca del paso
        if (paso != null)
        {
            float distancia = Vector2.Distance(transform.position, paso.transform.position);

            if (distancia <= distanciaPaso)
            {
                bool mirandoDerecha = !spriteRenderer.flipX;
                bool pasoALaDerecha = paso.transform.position.x > transform.position.x;

                bool mirandoFrente = (mirandoDerecha && pasoALaDerecha) || (!mirandoDerecha && !pasoALaDerecha);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        // Llama al script de ataque si lo tienes
        GetComponent<PlayerAttack>()?.TryAttack();
    }
}