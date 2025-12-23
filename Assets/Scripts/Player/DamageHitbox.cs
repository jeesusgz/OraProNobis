using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        // Buscamos el PlayerController en el padre
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player HITBOX colisionó con: " + collision.name);

        EnemyHealth enemy = collision.GetComponentInParent<EnemyHealth>();
        if (enemy != null && playerController != null)
        {
            int currentDamage = playerController.dañoActual;
            Debug.Log("ENEMIGO DETECTADO → Aplicando daño: " + currentDamage);
            enemy.TakeDamage(currentDamage, transform);
        }
    }
}