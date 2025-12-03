using UnityEngine;

public class NazarenoAttack : MonoBehaviour
{
    public int damage = 1;
    public float hitCooldown = 0.5f;
    private float hitTimer = 0f;

    private void Update()
    {
        if (hitTimer > 0)
            hitTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemigo")) return;

        if (hitTimer <= 0)
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.root);
                // transform.root = el nazareno real, no el objeto del trigger
            }

            hitTimer = hitCooldown;
        }
    }
}
