using UnityEngine;
using System.Collections;

public class NazarenoHealthSystem : MonoBehaviour
{
    [Header("Vida del Nazareno")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Invulnerabilidad al recibir daño")]
    public float invulnerabilityTime = 0.5f;
    public float flashSpeed = 0.1f;

    private bool isInvulnerable = false;
    private bool isDying = false;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] scripts;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        scripts = GetComponents<MonoBehaviour>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDying)
            return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió daño. Vida: " + currentHealth);

        StartCoroutine(InvulnerabilityCoroutine());

        if (currentHealth <= 0)
            StartCoroutine(DieRoutine());
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        float timer = 0f;

        while (timer < invulnerabilityTime)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashSpeed);

                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(flashSpeed);
            }

            timer += flashSpeed * 2f;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        isInvulnerable = false;
    }

    IEnumerator DieRoutine()
    {
        isDying = true;

        Debug.Log(gameObject.name + " está muriendo...");

        // Animación de muerte
        if (anim != null)
            anim.SetTrigger("Die");

        // Desactivamos todos los scripts excepto este
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        // Tiempo para que acabe la animación
        yield return new WaitForSeconds(0.8f);

        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Sube la vida máxima del nazareno en 2 unidades y actualiza la vida actual
    /// </summary>
    public void SubirNivelVida()
    {
        maxHealth += 2;
        currentHealth += 2; // También aumenta la vida actual

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log(gameObject.name + " subió de nivel. Nueva vida máxima: " + maxHealth);
    }
}