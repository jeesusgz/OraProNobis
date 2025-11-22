using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;

    private int currentHealth;
    private Rigidbody2D rb;

    [HideInInspector] public bool isKnockedBack = false;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        currentHealth -= amount;

        if (rb != null)
            StartCoroutine(DoKnockback(attacker));

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator DoKnockback(Transform attacker)
    {
        isKnockedBack = true;

        // Dirección horizontal pura
        float dir = Mathf.Sign(transform.position.x - attacker.position.x);
        Vector2 knockVector = new Vector2(dir, 0f);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockVector * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackTime);

        isKnockedBack = false;
    }

    void Die()
    {
        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
