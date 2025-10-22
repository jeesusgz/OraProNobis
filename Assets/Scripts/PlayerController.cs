using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Internal")]
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Vector2 moveDirection;

    [Header("Movimiento")]
    public float speed;
    public float jumpForce;

    [Header("Input")]
    public InputActionReference move;
    public InputActionReference jump;

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
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}