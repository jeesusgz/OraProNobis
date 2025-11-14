using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PasoController : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaActivacion = 2f;
    public Transform jugador;
    public bool jugadorCerca = false;
    public InputActionReference levantarAction;

    private Animator animator;
    private bool animado = false;
    private bool levantado = false;

    [Header("Movimiento hacia la capilla")]
    public float moveSpeed = 1f;
    public Transform targetPoint;
    private Rigidbody2D rb;

    private bool puedeMoverse = false;
    private bool entrando = false;
    public float arriveThreshold = 0.3f;  // <-- más seguro

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Collider2D mainCollider;

    private void OnEnable()
    {
        if (levantarAction != null)
        {
            levantarAction.action.Enable();
            levantarAction.action.performed += OnLevantar;
        }
    }

    private void OnDisable()
    {
        if (levantarAction != null)
        {
            levantarAction.action.performed -= OnLevantar;
            levantarAction.action.Disable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider2D>();
    }

    private void OnLevantar(InputAction.CallbackContext context)
    {
        if (!jugadorCerca || animado) return;

        animado = true;

        if (!levantado)
        {
            animator.SetTrigger("Levanta");
            levantado = true;
            puedeMoverse = false;
            Invoke(nameof(ActivarMovimiento), 2f);
        }
        else
        {
            animator.SetTrigger("Arria");
            levantado = false;
            puedeMoverse = false;
        }

        Invoke(nameof(FinAnimacion), 3f);
    }

    void Update()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!entrando && targetPoint != null)
        {
            float dist = Mathf.Abs(transform.position.x - targetPoint.position.x);

            if (dist <= arriveThreshold)
            {
                EntrarCapilla();
            }
        }
    }

    void FixedUpdate()
    {
        if (levantado && puedeMoverse && targetPoint != null && !entrando)
        {
            Vector2 next = Vector2.MoveTowards(
                rb.position,
                new Vector2(targetPoint.position.x, rb.position.y),
                moveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(next);

            animator.SetBool("Andando", true);
        }
        else
        {
            animator.SetBool("Andando", false);
        }
    }

    private void EntrarCapilla()
    {
        if (entrando) return;
        entrando = true;

        puedeMoverse = false;
        levantado = false;

        animator.SetBool("Andando", false);

        // desactivar colisiones
        if (mainCollider != null)
            mainCollider.enabled = false;

        // prevenir movimientos raros
        rb.bodyType = RigidbodyType2D.Kinematic;

        // FADE + DESTRUCCIÓN
        StartCoroutine(FadeAndDestroy(1.5f));
    }

    private void ActivarMovimiento()
    {
        puedeMoverse = true;
    }

    public void FinAnimacion()
    {
        animado = false;
    }

    private IEnumerator FadeAndDestroy(float duration)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            foreach (var sr in sprites)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    sr.color = new Color(c.r, c.g, c.b, alpha);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // asegurar alpha 0
        foreach (var sr in sprites)
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}