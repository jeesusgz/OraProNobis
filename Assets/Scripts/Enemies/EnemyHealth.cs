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

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        if (isDying) return;

        currentHealth -= amount;

        StartCoroutine(DamageFlash());

        if (rb != null)
            StartCoroutine(DoKnockback(attacker));

        if (currentHealth <= 0)
            StartCoroutine(DieRoutine()); // ← ← AQUI ESTABA EL ERROR
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

        if (anim != null)
            anim.SetTrigger("Die");

        // Espera a que se reproduzca la animación
        yield return new WaitForSeconds(0.10f);

        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
