using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;

    private int currentHealth;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDying = false;

    [HideInInspector] public bool isKnockedBack = false;

    private GhostController ghost; // ← detectar si este enemigo es un fantasma

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        ghost = GetComponent<GhostController>(); // ← si es fantasma, será distinto de null
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        if (isDying) return;

        currentHealth -= amount;

        if (ghost != null)
            ghost.PlayHitFlash(); // fantasma usa su propio flash
        else
            StartCoroutine(DamageFlash()); // otros enemigos

        if (rb != null && ghost == null)
            StartCoroutine(DoKnockback(attacker));

        if (currentHealth <= 0)
        {
            if (ghost != null)
                ghost.PlayDeath(); // fantasma usa muerte propia
            else
                StartCoroutine(DieRoutine()); // enemigo normal
        }
    }

    IEnumerator DoKnockback(Transform attacker)
    {
        isKnockedBack = true;

        float dir = Mathf.Sign(transform.position.x - attacker.position.x);
        Vector2 knockVector = new Vector2(dir, 0f);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockVector * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackTime);

        isKnockedBack = false;
    }

    IEnumerator DamageFlash()
    {
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }

    IEnumerator DieRoutine()
    {
        isDying = true;

        // 1. Parar movimiento
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // 2. Cortar animaciones previas
        if (anim != null)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("isMoving", false);
            anim.SetTrigger("Die");
        }

        // 3. Desactivar IA y movimiento (pero NO EnemyHealth)
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        // 4. Esperar animación
        yield return new WaitForSeconds(0.8f);

        // 5. Destruir enemigo o su padre
        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
