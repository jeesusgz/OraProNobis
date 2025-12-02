using System.Collections;
using UnityEngine;

public class NazarenoController : MonoBehaviour
{
    public PasoController paso;      // asignado por el spawner
    public float offset = 1f;        // distancia inicial al spawn
    public Transform destinoCapilla;
    public int damage = 1;

    private Animator animator;
    private bool eliminado = false;
    private float initialXOffset;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Guardamos la distancia inicial al paso para mantenerla siempre
        if (paso != null)
            initialXOffset = transform.position.x - paso.transform.position.x;
    }

    private void Update()
    {
        if (eliminado || paso == null) return;

        // Solo se mueve si el paso está levantado y puede moverse
        if (paso.Levantado && paso.CurrentStamina > 0f && paso.Animado == false)
        {
            float moveStep = paso.moveSpeed * Time.deltaTime;

            // Movemos el nazareno sumando exactamente lo que se mueve el paso
            transform.position += new Vector3(moveStep, 0, 0);

            if (animator != null)
                animator.SetBool("walking", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("walking", false);
        }

        // Llegó a la capilla → fade out
        if (destinoCapilla != null && Mathf.Abs(transform.position.x - destinoCapilla.position.x) < 0.2f)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        eliminado = true;
        if (animator != null)
            animator.SetBool("walking", false);

        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        float t = 0;
        float duration = 0.7f;

        while (t < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            foreach (var sr in srs)
            {
                if (sr != null)
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            }
            t += Time.deltaTime;
            yield return null;
        }

        paso.NotifyNazarenoDeath(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
            enemy.TakeDamage(damage, transform);
    }
}
