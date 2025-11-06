using UnityEngine;
using UnityEngine.InputSystem;

public class PasoController : MonoBehaviour
{
    [Header("Configuración")]
    public float distanciaActivacion = 2f;
    public Transform jugador;
    public bool jugadorCerca = false; // ✅ Añadido para que ZonaInteraccion funcione
    public InputActionReference levantarAction;

    private Animator animator;
    private bool animado = false;
    private bool levantado = false;

    public float moveSpeed = 1f;
    public Transform targetPoint; // la capilla
    private Rigidbody2D rb;

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

        // Solo permite levantar si el jugador está cerca
        if (!jugadorCerca)
        {
            Debug.Log("Jugador demasiado lejos del paso.");
            return;
        }

        if (animado || jugador == null)
        {
            Debug.Log("Bloqueado por animado o jugador nulo");
            return;
        }

        if (!levantado)
        {
            Debug.Log("Activando Levanta");
            animator.SetTrigger("Levanta");
            levantado = true;
        }
        else
        {
            Debug.Log("Activando Bajar");
            animator.SetTrigger("Arria");
            levantado = false;
        }

        animado = true;
        Invoke(nameof(FinAnimacion), 2f); // tras 2s permite nueva acción
    }

    void FixedUpdate()
    {
        if (targetPoint != null)
        {
            Vector2 direction = (targetPoint.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void FinAnimacion()
    {
        animado = false;
    }
}
