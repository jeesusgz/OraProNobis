using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitbox;       // Un hijo del player con un collider trigger desactivado
    public float attackDuration = 0.1f;
    public float attackCooldown = 0.3f;

    private bool canAttack = true;

    public void TryAttack()
    {
        if (canAttack)
            StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        canAttack = false;

        Debug.Log("HITBOX ACTIVADA");
        hitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        Debug.Log("HITBOX DESACTIVADA");
        hitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
