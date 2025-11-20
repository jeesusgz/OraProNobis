using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float knockbackForce = 5f;

    private int currentHealth;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        currentHealth -= amount;

        // Knockback
        if (rb != null)
        {
            Vector2 direction = (transform.position - attacker.position).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
