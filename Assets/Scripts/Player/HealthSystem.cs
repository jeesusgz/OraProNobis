using UnityEngine;
using System;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action<int> OnMaxHealthChanged;

    [Header("Vida")]
    public int baseMaxHealth = 6;
    public int maxHealth;
    public int currentHealth;

    [Header("Audio de daño")]
    public AudioClip dañoClip;         
    public AudioSource audioSourceDaño;

    [Header("Modos especiales")]
    public bool infiniteHealth = false;

    public float invulnerabilityTime = 1f;
    public float flashSpeed = 0.1f;

    private bool isInvulnerable = false;
    private bool isPlayer = false;
    private bool isDying = false;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private MonoBehaviour[] scripts;

    [Header("Referencias UI")]
    public HeartManager playerHealthHeartManager;

    void Awake()
    {
        // Configuramos maxHealth antes de Start
        maxHealth = baseMaxHealth;

        if (CurrencyManager.Instance != null)
        {
            int nivel = CurrencyManager.Instance.gameData.vidaJugadorNivel;
            maxHealth += nivel;
        }

        currentHealth = maxHealth;

        // Notificamos la UI desde el inicio
        OnMaxHealthChanged?.Invoke(maxHealth);
        OnPlayerDamaged?.Invoke();
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        isPlayer = CompareTag("Player");
        scripts = GetComponents<MonoBehaviour>();

        // Inicializamos corazones en HeartManager
        if (playerHealthHeartManager != null)
            playerHealthHeartManager.InicializarHearts();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable || isDying)
            return;

        if (!infiniteHealth)
            currentHealth -= damageAmount;

        currentHealth = Mathf.Max(currentHealth, 0);

        
        if (audioSourceDaño != null && dañoClip != null)
        {
            audioSourceDaño.PlayOneShot(dañoClip);
        }

        OnPlayerDamaged?.Invoke();
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

        isInvulnerable = false;
    }

    IEnumerator DieRoutine()
    {
        isDying = true;

        // Activar animación de muerte
        if (isPlayer && anim != null)
            anim.SetTrigger("Die");

        // Cambiar música de fondo a la de muerte
        MusicManager.Instance?.CambiarAMusicaMuerte();

        // Desactivar otros scripts
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        if (isPlayer)
            FindObjectOfType<DeathMenu>()?.MostrarMenuMuerte();
    }

    // Subir la vida máxima y actualizar UI
    public void SubirNivelVida(int cantidad = 1)
    {
        maxHealth += cantidad;
        currentHealth += cantidad;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnMaxHealthChanged?.Invoke(maxHealth);
        OnPlayerDamaged?.Invoke();
    }
}