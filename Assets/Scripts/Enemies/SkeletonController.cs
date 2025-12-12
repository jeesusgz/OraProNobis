using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform paso;
    public Transform player;
    private EnemyHealth enemyHealth;

    [Header("Movimiento")]
    public float speed = 2.5f;

    [Header("Ataque")]
    public float rangoDeteccionJugador = 4f;

    [Header("Audio de pasos")]
    public AudioClip pasoClip;
    public AudioSource audioSourcePasos;
    public float intervaloEntrePasos = 0.5f;
    private float tiempoDesdeUltimoPaso = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        if (enemyHealth != null && enemyHealth.isKnockedBack)
        {
            // Evitar que la IA cancele el knockback
            anim.SetBool("Walking", false);
            return;
        }

        if (paso == null || player == null)
            return;

        Transform objetivo = paso;

        float distanciaJugador = Vector2.Distance(transform.position, player.position);
        if (distanciaJugador <= rangoDeteccionJugador)
            objetivo = player;

        MoverHacia(objetivo.position);

        // Sonido de pasos
        if (anim.GetBool("Walking"))
        {
            tiempoDesdeUltimoPaso += Time.fixedDeltaTime;
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
            tiempoDesdeUltimoPaso = intervaloEntrePasos; // reinicia contador cuando no camina
        }
    }

    void MoverHacia(Vector2 objetivo)
    {
        float dir = objetivo.x - transform.position.x;
        float velX = Mathf.Sign(dir) * speed;

        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

        if (sr != null)
            sr.flipX = dir > 0;

        if (anim != null)
            anim.SetBool("Walking", Mathf.Abs(velX) > 0.1f);
    }
}