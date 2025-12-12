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

    [Header("Audio de pasos")]
    public AudioClip pasoClip;
    public AudioSource audioSourcePasos; // asigna un AudioSource en el inspector
    public float intervaloEntrePasos = 0.4f; // segundos entre pasos
    private float tiempoDesdeUltimoPaso = 0f;

    [Header("Audio de salto")]
    public AudioClip saltoClip;
    public AudioSource audioSourceSalto; // asigna un AudioSource en el inspector

    [Header("Audio de monedas")]
    public AudioClip monedaClip;
    public AudioSource audioSourceMoneda;

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

    [Header("Enemigos")]
    public float bounceForceOnEnemy = 12f;

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

        int nivelVelocidad = CurrencyManager.Instance.gameData.jugadorVelocidadBotonNivel;
        speed = 5f + (nivelVelocidad * 1f);
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
            animator.SetBool("IsJumping", false);
        }
        else if (wasGrounded && !isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }
        wasGrounded = isGrounded;

        if (isGrounded && jump.action.WasPerformedThisFrame())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }

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

        // Sonido de pasos
        if (isGrounded && Mathf.Abs(moveDirection.x) > 0.1f)
        {
            tiempoDesdeUltimoPaso += Time.deltaTime;
            if (tiempoDesdeUltimoPaso >= intervaloEntrePasos)
            {
                if (audioSourcePasos != null && pasoClip != null)
                {
                    audioSourcePasos.PlayOneShot(pasoClip);
                }
                tiempoDesdeUltimoPaso = 0f;
            }
        }
        else
        {
            // Reinicia el contador si no se está moviendo
            tiempoDesdeUltimoPaso = intervaloEntrePasos;
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

            // Reproducir sonido de salto
            if (audioSourceSalto != null && saltoClip != null)
            {
                audioSourceSalto.PlayOneShot(saltoClip);
            }

            animator.SetTrigger("Jump");
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        GetComponent<PlayerAttack>()?.TryAttack();
    }

    public void SubirVelocidad()
    {
        // Sube +1 de speed por nivel
        speed += 1f;
    }

    public void RecogerMoneda()
    {
        if (audioSourceMoneda != null && monedaClip != null)
        {
            audioSourceMoneda.PlayOneShot(monedaClip);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto con el que colisionamos tiene el Tag "Enemigo"
        if (!collision.collider.CompareTag("Enemigo"))
            return;

        // Comprueba la dirección de la colisión usando los puntos de contacto
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // La normal de contacto indica la dirección perpendicular a la superficie de colisión.
            // Si contact.normal.y es un valor positivo alto (cercano a 1), significa que el jugador fue golpeado desde abajo, 
            // o lo que es lo mismo, el jugador aterrizó sobre el enemigo.

            float dot = Vector2.Dot(contact.normal, Vector2.up);

            // Umbral para decidir si es una colisión "superior". 
            // 0.8f funciona bien para superficies planas superiores.
            if (dot > 0.8f)
            {
                // *** ¡SOLUCIÓN APLICADA AQUÍ! ***
                // Reiniciamos la velocidad vertical actual y aplicamos una fuerza de salto hacia arriba (Impulse).

                // 1. Opcional: Ejecuta la lógica para "matar" o "dañar" al enemigo aquí, si corresponde.
                // collision.gameObject.GetComponent<EnemyScript>()?.TakeDamage(); 

                // 2. Aplica el rebote al jugador:
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reinicia velocidad Y actual
                rb.AddForce(Vector2.up * bounceForceOnEnemy, ForceMode2D.Impulse); // Aplica rebote

                // Opcional: Activa una animación de salto/rebote
                animator.SetTrigger("Jump");

                // Ya hemos procesado el rebote, podemos salir del método
                return;
            }
        }
    }
}