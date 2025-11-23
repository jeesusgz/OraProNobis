using UnityEngine;

public class HitboxDamageRelay : MonoBehaviour
{
    private EnemyHealth parentHealth;

    private void Start()
    {
        parentHealth = GetComponentInParent<EnemyHealth>();
    }

    public void ApplyDamage(int dmg, Transform attacker)
    {
        if (parentHealth != null)
            parentHealth.TakeDamage(dmg, attacker);
    }
}
