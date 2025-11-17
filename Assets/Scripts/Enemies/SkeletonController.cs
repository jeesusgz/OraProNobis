using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform paso;
    public Transform player;

    [Header("Movimiento")]
    public float speed = 2.5f;

    [Header("Ataque")]
    public float rangoDeteccionJugador = 4f; // distancia para atacar al jugador

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (paso == null || player == null)
            return;

        Transform objetivo = paso;

        // Si el jugador está cerca, atacarlo
        float distanciaJugador = Vector2.Distance(transform.position, player.position);
        if (distanciaJugador <= rangoDeteccionJugador)
            objetivo = player;

        MoverHacia(objetivo.position);
    }

    void MoverHacia(Vector2 objetivo)
    {
        float dir = objetivo.x - transform.position.x;
        float velX = Mathf.Sign(dir) * speed;

        // Solo modificar la velocidad horizontal, gravedad queda intacta
        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

        // Flip del sprite
        if (sr != null)
            sr.flipX = dir > 0;

        // Animación de caminar
        if (anim != null)
            anim.SetBool("Walking", Mathf.Abs(velX) > 0.1f);
    }
}
