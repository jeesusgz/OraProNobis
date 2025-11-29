using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PasoHealthSystem : MonoBehaviour
{
    [Header("Vida del Paso")]
    public int maxHealth = 30;
    public int currentHealth;

    [Header("UI")]
    public Image healthBar; // Asigna la barra de vida desde el Inspector

    [Header("Modos especiales")]
    public bool infiniteHealth = false;

    public float invulnerabilityTime = 1f;
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

        UpdateUI();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable || isDying)
            return;

        if (!infiniteHealth)
            currentHealth -= damageAmount;

        Debug.Log(gameObject.name + " ha recibido daño. Vida: " + currentHealth);

        UpdateUI();

        StartCoroutine(InvulnerabilityCoroutine());

        if (!infiniteHealth && currentHealth <= 0)
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

        if (anim != null)
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
        Debug.Log(gameObject.name + " paso destruido");
        FindObjectOfType<DeathMenu>().MostrarMenuMuerte();
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}