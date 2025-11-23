using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player HITBOX colisionó con: " + collision.name);

        // Buscar EnemyHealth en el objeto impactado o en sus padres
        EnemyHealth enemy = collision.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("ENEMIGO DETECTADO → Aplicando daño");
            enemy.TakeDamage(damage, transform);
        }
    }
}
