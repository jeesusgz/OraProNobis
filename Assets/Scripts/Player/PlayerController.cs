using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Input System")]
    public InputActionReference move;
    public InputActionReference jump;

    private Vector2 moveDirection;

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        jump.action.performed += OnJump;
    }

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        jump.action.performed -= OnJump;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();

        // Flip del personaje
        if (moveDirection.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveDirection.x < -0.1f)
            spriteRenderer.flipX = true;

        // Control de caída y salto
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jump.action.IsPressed())
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
}