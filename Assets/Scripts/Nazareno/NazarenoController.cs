using System.Collections;
using UnityEngine;

public class NazarenoController : MonoBehaviour
{
    public PasoController paso;
    public NazarenoController lider;
    public Transform destinoCapilla;
    public bool delante;
    public int damage = 1;

    [Header("Offsets y Separación")]
    public float extraGap = 0.05f;
    public float offsetY = 0f;  // ← AJUSTA ESTE VALOR (prueba 0.2, 0.3, etc.)

    private Animator animator;
    private bool eliminado = false;

    [HideInInspector] public BoxCollider2D myCollider;
    private float distanciaSeguridad = 0.05f;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        if (myCollider != null)
            myCollider.isTrigger = true;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    // ==== CÁLCULO DE OFFSETS REALES ====
    private float GetOffsetRespectoPaso()
    {
        if (paso == null) return 0f;

        Collider2D colPaso = paso.GetComponent<Collider2D>();
        float anchoPaso = colPaso != null ? colPaso.bounds.size.x : 1f;
        float anchoNaz = myCollider != null ? myCollider.bounds.size.x : 0.5f;

        float offsetBase = (anchoPaso * 0.5f) + (anchoNaz * 0.5f) + extraGap;
        return delante ? offsetBase : -offsetBase;
    }

    private float GetSeparationRespectoLider()
    {
        float anchoLider = lider != null && lider.myCollider != null ? lider.myCollider.bounds.size.x : 0.5f;
        float anchoNaz = myCollider != null ? myCollider.bounds.size.x : 0.5f;

        return (anchoLider * 0.5f) + (anchoNaz * 0.5f) + distanciaSeguridad;
    }

    private void FixedUpdate()
    {
        if (eliminado || paso == null) return;

        Vector3 objetivo;

        if (lider != null)
        {
            float separation = GetSeparationRespectoLider();
            float dir = delante ? 1f : -1f;
            objetivo = lider.transform.position + new Vector3(separation * dir, 0f, 0f);
        }
        else
        {
            float offset = GetOffsetRespectoPaso();
            objetivo = paso.transform.position + new Vector3(offset, 0f, 0f);
        }

        // 🎯 SOLUCIÓN DEFINITIVA: Y fija del suelo, IGNORA si el paso está levantado
        objetivo.y = paso.transform.position.y - (paso.Levantado ? 0.25f : 0f);

        if (paso.Levantado && paso.CurrentStamina > 0f && !paso.Animado)
        {
            float speedMultiplier = paso.CurrentStamina / paso.maxStamina < 0.2f ? paso.lowStaminaMultiplier : 1f;
            float followerSpeedFactor = 1.2f;
            float step = paso.moveSpeed * speedMultiplier * followerSpeedFactor * Time.fixedDeltaTime;

            float dist = Vector3.Distance(transform.position, objetivo);

            if (dist < 0.01f)
            {
                transform.position = objetivo;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, objetivo, step);
            }

            if (animator != null)
                animator.SetBool("walking", dist > 0.005f);
        }
        else
        {
            if (animator != null)
                animator.SetBool("walking", false);
            transform.position = objetivo;
        }

        if (destinoCapilla != null && Mathf.Abs(transform.position.x - destinoCapilla.position.x) < 0.2f)
            StartCoroutine(FadeOutAndDestroy());
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

        if (paso != null)
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
