using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLISION HITBOX CON: " + collision.name);

        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(1, transform); // Daño + knockback
            Debug.Log("DAÑO APLICADO!");
        }
    }
}
