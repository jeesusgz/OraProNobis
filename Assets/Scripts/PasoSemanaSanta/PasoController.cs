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
    public Transform targetPoint; // La capilla (destino)
    private Rigidbody2D rb;

    private bool puedeMoverse = false; // 🔹 Controla si el paso puede empezar a moverse

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
    }

    private void OnLevantar(InputAction.CallbackContext context)
    {
        Debug.Log("E pulsada");

        if (!jugadorCerca)
        {
            Debug.Log("Jugador demasiado lejos del paso.");
            return;
        }

        if (animado)
        {
            Debug.Log("Esperando a que termine animación.");
            return;
        }

        animado = true;

        if (!levantado)
        {
            Debug.Log("Activando animación de Levanta");
            animator.SetTrigger("Levanta");
            levantado = true;

            // Espera 2 segundos para permitir movimiento tras la levantá
            puedeMoverse = false;
            Invoke(nameof(ActivarMovimiento), 2f);
        }
        else
        {
            Debug.Log("Activando animación de Bajar");
            animator.SetTrigger("Arria");
            levantado = false;
            puedeMoverse = false;
        }

        // Finaliza el estado de animación bloqueada tras 3 segundos
        Invoke(nameof(FinAnimacion), 3f);
    }

    void FixedUpdate()
    {
        if (levantado && puedeMoverse && targetPoint != null)
        {
            Vector2 direction = (targetPoint.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            animator.SetBool("Andando", true);
        }
        else
        {
            animator.SetBool("Andando", false);
        }
    }

    private void ActivarMovimiento()
    {
        puedeMoverse = true; // 🔹 El paso ya puede empezar a moverse tras unos segundos
    }

    public void FinAnimacion()
    {
        animado = false;
    }
}
