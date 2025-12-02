using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damageAmount = 1;
    public string targetTag1 = "Paso";
    public string targetTag2 = "Player";
    public string targetTag3 = "Nazareno";

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
        // Comparar tags
        if (obj.CompareTag(targetTag1) || obj.CompareTag(targetTag2) || obj.CompareTag(targetTag3))
        {
            // Player
            HealthSystem playerHealth = obj.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                return;
            }

            // Paso
            PasoHealthSystem pasoHealth = obj.GetComponent<PasoHealthSystem>();
            if (pasoHealth != null)
            {
                pasoHealth.TakeDamage(damageAmount);
                return;
            }

            // Nazareno
            NazarenoHealthSystem nazHealth = obj.GetComponent<NazarenoHealthSystem>();
            if (nazHealth != null)
            {
                nazHealth.TakeDamage(damageAmount);
                return;
            }
        }
    }
}
