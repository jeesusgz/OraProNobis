using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitbox;
    public float attackDuration = 0.1f;
    public float attackCooldown = 0.3f;

    private bool canAttack = true;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TryAttack()
    {
        if (canAttack)
            StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        canAttack = false;

        // 👉 Cambiar dirección de la hitbox según flipX
        if (sr.flipX)
            hitbox.transform.localPosition = new Vector3(-0.8f, hitbox.transform.localPosition.y, 0);
        else
            hitbox.transform.localPosition = new Vector3(0.8f, hitbox.transform.localPosition.y, 0);

        Debug.Log("HITBOX ACTIVADA");
        hitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        Debug.Log("HITBOX DESACTIVADA");
        hitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
