using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitbox;
    public float hitboxActiveTime = 0.1f; // Duración del golpe
    public float attackCooldown = 0.10f;

    private bool canAttack = true;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void TryAttack()
    {
        if (canAttack)
        {
            canAttack = false;
            anim.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
        }
    }

    // Esta función será llamada desde el evento de animación
    public void ActivateHitbox()
    {
        float xOffset = sr.flipX ? -0.8f : 0.8f;
        hitbox.transform.localPosition = new Vector3(xOffset, hitbox.transform.localPosition.y, 0);
        hitbox.SetActive(true);
        StartCoroutine(DeactivateHitbox());
        Debug.Log("HITBOX ACTIVADA");
    }

    private IEnumerator DeactivateHitbox()
    {
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.SetActive(false);
        Debug.Log("HITBOX DESACTIVADA");
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
