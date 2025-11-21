using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Hitbox")]
    public GameObject hitbox;       // Objeto hijo con collider trigger
    public float hitboxOffset = 0.5f;   // Distancia a cada lado

    [Header("Ataque")]
    public float attackDuration = 0.1f;
    public float attackCooldown = 0.3f;

    private bool canAttack = true;

    private SpriteRenderer spriteRenderer;
    private Transform hitboxTransform;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitboxTransform = hitbox.transform;
    }

    private void Update()
    {
        UpdateHitboxDirection();
    }

    // Cambia la posición de la hitbox según flipX
    private void UpdateHitboxDirection()
    {
        if (spriteRenderer.flipX)
            hitboxTransform.localPosition = new Vector3(-hitboxOffset, 0, 0);
        else
            hitboxTransform.localPosition = new Vector3(hitboxOffset, 0, 0);
    }

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
