using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damageAmount = 1;
    public string targetTag1 = "Paso";
    public string targetTag2 = "Player";

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        TryDamage(collider.gameObject);
    }

    void TryDamage(GameObject obj)
    {
        if (obj.CompareTag(targetTag1) || obj.CompareTag(targetTag2))
        {
            HealthSystem health = obj.GetComponent<HealthSystem>();

            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
