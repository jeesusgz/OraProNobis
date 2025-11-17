using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform paso;
    public Transform player;

    [Header("Movimiento")]
    public float speed = 3f;
    public float alturaFlotacion = 0.5f;   // cuánto sube y baja el fantasma al flotar
    public float velocidadFlotacion = 2f;  // velocidad del movimiento de flotación

    [Header("Detección / Ataque")]
    public float rangoDeteccionJugador = 4f; // si el jugador está cerca, se gira hacia él
    public float rangoAtaqueJugador = 2f;    // si quieres ataque luego

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float flotacionOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // El fantasma NO usa gravedad
        if (rb != null)
        {
            rb.gravityScale = 0;
        }

        // Desfase aleatorio en la animación de flotación
        flotacionOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        if (paso == null || player == null)
            return;

        Flotar();

        float distanciaJugador = Vector2.Distance(transform.position, player.position);

        // Si el jugador está cerca, solo se gira hacia él (no cambia el objetivo)
        if (distanciaJugador <= rangoDeteccionJugador)
        {
            MirarJugador();
        }

        // Mover hacia el paso SIEMPRE
        MoverHaciaPaso();
    }

    void MoverHaciaPaso()
    {
        Vector2 objetivo = paso.position;
        Vector2 pos = Vector2.MoveTowards(transform.position, objetivo, speed * Time.deltaTime);
        transform.position = pos;

        // Voltear sprite según dirección
        if (objetivo.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;

        // Activar animación de movimiento
        if (anim != null)
            anim.SetBool("isMoving", true);
    }

    void MirarJugador()
    {
        if (player.position.x > transform.position.x)
            sr.flipX = false;
        else
            sr.flipX = true;
    }

    void Flotar()
    {
        // Movimiento vertical suave tipo "fantasma"
        float y = Mathf.Sin(Time.time * velocidadFlotacion + flotacionOffset) * alturaFlotacion;
        transform.position += new Vector3(0, y * Time.deltaTime, 0);
    }
}
