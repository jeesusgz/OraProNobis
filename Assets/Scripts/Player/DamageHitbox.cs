using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLISIONÓ CON: " + collision.name);

        EnemyHealth enemy = collision.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("ENEMIGO DETECTADO → HACIENDO DAÑO");
            enemy.TakeDamage(damage, transform);
        }
    }
}
