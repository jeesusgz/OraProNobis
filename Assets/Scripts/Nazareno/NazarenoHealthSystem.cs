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
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        scripts = GetComponents<MonoBehaviour>();

        // 🔥 Aplicar mejora guardada en GameData
        int nivel = CurrencyManager.Instance.gameData.vidaNazarenoNivel;

        maxHealth = 3 + (nivel * 2);   // cada nivel suma +2 (igual que SubirNivelVida)
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDying) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " recibió daño. Vida: " + currentHealth);

        StartCoroutine(InvulnerabilityCoroutine());

        if (currentHealth <= 0) StartCoroutine(DieRoutine());
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

        if (spriteRenderer != null) spriteRenderer.color = Color.white;
        isInvulnerable = false;
    }

    IEnumerator DieRoutine()
    {
        isDying = true;

        if (anim != null) anim.SetTrigger("Die");

        foreach (var s in scripts)
            if (s != this) s.enabled = false;

        yield return new WaitForSeconds(0.8f);

        // Llamar solo a su propio controlador
        NazarenoController nc = GetComponent<NazarenoController>();
        if (nc != null)
            StartCoroutine(nc.FadeOutAndDestroy());
        else
            Destroy(gameObject);
    }

    void Die() => Destroy(gameObject);

    /// <summary>
    /// Sube la vida máxima del nazareno en 2 unidades y actualiza la vida actual
    /// </summary>
    public void SubirNivelVida()
    {
        maxHealth += 2;
        currentHealth += 2;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        Debug.Log(gameObject.name + " subió de nivel. Nueva vida máxima: " + maxHealth);
    }
}