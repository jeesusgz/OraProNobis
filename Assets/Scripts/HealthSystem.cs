using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float invulnerabilityTime = 1f;
    public float flashSpeed = 0.1f;

    private bool isInvulnerable = false;
    private bool isPlayer = false;
    private bool isDying = false;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] scripts;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        isPlayer = CompareTag("Player");
        scripts = GetComponents<MonoBehaviour>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable || isDying)
            return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido daño. Vida: " + currentHealth);

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

        if (isPlayer && anim != null)
            anim.SetTrigger("Die");

        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        Die();
    }

    void Die()
    {
        if (isPlayer)
        {
            Debug.Log("PLAYER MUERTO");
            // TODO: respawn o pantalla de game over
        }
        else
        {
            Debug.Log(gameObject.name + " enemigo/paso destruido");
            Destroy(gameObject);
        }
    }
}
