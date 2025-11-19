using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float invulnerabilityTime = 1f; // Tiempo de invulnerabilidad
    private bool isInvulnerable = false;   // Controla si puede recibir daño

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        // Si está invulnerable, no recibe daño
        if (isInvulnerable)
            return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida actual: " + currentHealth);

        // Inicia invulnerabilidad
        StartCoroutine(InvulnerabilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        // Opcional: hacer parpadear al personaje
        // TODO: agregar efecto visual si quieres

        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        Destroy(gameObject);
    }
}
