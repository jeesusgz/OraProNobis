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
    public float arriveThreshold = 0.3f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Collider2D mainCollider;

    [Header("Estamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSecond = 20f;
    public float staminaRecoveryPerSecond = 10f;
    public float fullDrainPenaltyTime = 2f; // segundos de penalización
    public float lowStaminaMultiplier = 0.5f; // velocidad reducida al estar baja

    [Header("UI del Paso")]
    public UnityEngine.UI.Image staminaBar;

    private bool fullDrainPenaltyActive = false;
    private float penaltyTimer = 0f;

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

        currentStamina = maxStamina;
    }

    private void OnLevantar(InputAction.CallbackContext context)
    {
        if (!jugadorCerca || animado || fullDrainPenaltyActive || currentStamina <= 0f)
            return;

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
        // Detección de suelo
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Penalización por estamina
        if (fullDrainPenaltyActive)
        {
            penaltyTimer -= Time.deltaTime;
            if (penaltyTimer <= 0f) fullDrainPenaltyActive = false;
        }

        // Recuperar estamina
        if (!animado && !fullDrainPenaltyActive)
            RecoverStamina();

        // Movimiento automático hacia la capilla
        if (!entrando && targetPoint != null)
        {
            float dist = Mathf.Abs(transform.position.x - targetPoint.position.x);
            if (dist <= arriveThreshold)
                EntrarCapilla();
        }

        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }

    void FixedUpdate()
    {
        if (levantado && puedeMoverse && targetPoint != null && !entrando)
        {
            float speedMultiplier = 1f;

            if (currentStamina / maxStamina < 0.2f)
                speedMultiplier = lowStaminaMultiplier;

            Vector2 next = Vector2.MoveTowards(
                rb.position,
                new Vector2(targetPoint.position.x, rb.position.y),
                moveSpeed * speedMultiplier * Time.fixedDeltaTime
            );

            rb.MovePosition(next);
            animator.SetBool("Andando", true);

            // Consumir estamina al moverse
            currentStamina -= staminaDrainPerSecond * Time.fixedDeltaTime;

            // ⚡ Si se vacía la estamina, forzar Arria
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                fullDrainPenaltyActive = true;
                penaltyTimer = fullDrainPenaltyTime;
                puedeMoverse = false;

                // Forzar animación de Arria si estaba levantado
                if (levantado)
                {
                    animator.SetTrigger("Arria");
                    levantado = false;
                    animado = true;
                    Invoke(nameof(FinAnimacion), 3f); // termina la animación
                }
            }
        }
    }

    void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryPerSecond * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }

    private void EntrarCapilla()
    {
        if (entrando) return;
        entrando = true;

        puedeMoverse = false;
        levantado = false;

        animator.SetBool("Andando", false);

        if (mainCollider != null)
            mainCollider.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;

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

    public bool Animado
    {
        get { return animado; }
    }

    public bool FullDrainPenaltyActive
    {
        get { return fullDrainPenaltyActive; }
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }

    public bool Levantado
    {
        get { return levantado; }
    }
}