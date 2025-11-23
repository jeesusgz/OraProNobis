using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform paso;
    public Transform player;

    [Header("Movimiento")]
    public float speed = 3f;
    public float alturaFlotacion = 0.5f;
    public float velocidadFlotacion = 2f;

    [Header("Detección / Ataque")]
    public float rangoDeteccionJugador = 4f;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float flotacionOffset;

    private bool isDying = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // El fantasma NO usa gravedad
        if (rb != null)
            rb.gravityScale = 0;

        flotacionOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        if (isDying) return; // ⛔ Detener todo si está muriendo
        if (paso == null || player == null) return;

        Flotar();

        float distanciaJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaJugador <= rangoDeteccionJugador)
            MirarJugador();

        MoverHaciaPaso();
    }

    void MoverHaciaPaso()
    {
        Vector2 objetivo = paso.position;
        Vector2 pos = Vector2.MoveTowards(transform.position, objetivo, speed * Time.deltaTime);
        transform.position = pos;

        // Voltear sprite según dirección
        sr.flipX = objetivo.x < transform.position.x;

        anim.SetBool("isMoving", true);
    }

    void MirarJugador()
    {
        sr.flipX = player.position.x < transform.position.x;
    }

    void Flotar()
    {
        float y = Mathf.Sin(Time.time * velocidadFlotacion + flotacionOffset) * alturaFlotacion;
        transform.position += new Vector3(0, y * Time.deltaTime, 0);
    }

    

    public void PlayHitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    IEnumerator HitFlashRoutine()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public void PlayDeath()
    {
        if (isDying) return;

        isDying = true;

        anim.SetBool("isMoving", false);
        anim.SetTrigger("Die");

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(0.6f); // duración de la animación
        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
