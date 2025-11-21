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

        // Knockback REAL según la posición del atacante
        if (rb != null)
        {
            float direction = Mathf.Sign(transform.position.x - attacker.position.x);
            // si el player está a la izquierda → +1 (derecha)
            // si está a la derecha → -1 (izquierda)

            Vector2 knockVector = new Vector2(direction, 0f);

            rb.AddForce(knockVector * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
