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

        // ⛔ NO knockback si lo golpea un nazareno
        if (rb != null && ghost == null && !attacker.CompareTag("Nazareno"))
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
        // ⛔ Bloque extra de seguridad (por si otro script llama a knockback)
        if (attacker.CompareTag("Nazareno"))
            yield break;

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

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (anim != null)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("isMoving", false);
            anim.SetTrigger("Die");
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        yield return new WaitForSeconds(0.8f);

        DropCoins();

        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }

    private void DropCoins()
    {
        int amount = Random.Range(minCoins, maxCoins + 1);

        for (int i = 0; i < amount; i++)
        {
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

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
