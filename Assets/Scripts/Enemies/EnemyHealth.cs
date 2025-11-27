using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;

    [Header("Drop de Monedas")]
    public int minCoins = 1;
    public int maxCoins = 3;
    public GameObject coinPrefab;

    private int currentHealth;
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDying = false;

    [HideInInspector] public bool isKnockedBack = false;

    private GhostController ghost;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        ghost = GetComponent<GhostController>();
    }

    public void TakeDamage(int amount, Transform attacker)
    {
        if (isDying) return;

        currentHealth -= amount;

        if (ghost != null)
            ghost.PlayHitFlash();
        else
            StartCoroutine(DamageFlash());

        if (rb != null && ghost == null)
            StartCoroutine(DoKnockback(attacker));

        if (currentHealth <= 0)
        {
            if (ghost != null)
            {
                ghost.PlayDeath();
                DropCoins();
            }
            else
            {
                StartCoroutine(DieRoutine());
            }
        }
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

        // 1. Parar movimiento
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // 2. Cortar animaciones previas
        if (anim != null)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("isMoving", false);
            anim.SetTrigger("Die");
        }

        // 3. Desactivar IA y movimiento (pero NO EnemyHealth)
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        // 4. Esperar animación
        yield return new WaitForSeconds(0.8f);

        DropCoins();

        // 5. Destruir enemigo o su padre
        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }

    private void DropCoins()
    {
        int amount = Random.Range(minCoins, maxCoins + 1);

        for (int i = 0; i < amount; i++)
        {
            // Instancia moneda física
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

                // Añadimos pequeño impulso aleatorio
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float forceX = Random.Range(-2f, 2f);
                    float forceY = Random.Range(2f, 4f);
                    rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
                }
            }
        }

        Debug.Log(gameObject.name + " soltó " + amount + " monedas");
    }
}
