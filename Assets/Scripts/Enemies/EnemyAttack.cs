using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 1;
    public float hitCooldown = 1f;
    private float timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Nazareno")) return;

        timer += Time.deltaTime;

        if (timer >= hitCooldown)
        {
            timer = 0;
            collision.GetComponent<NazarenoHealthSystem>()?.TakeDamage(damage);
        }
    }
}
