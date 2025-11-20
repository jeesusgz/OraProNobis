using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 0.7f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public InputActionReference attackAction;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackAction.action.performed += ctx => Attack();
    }

    private void OnEnable()
    {
        attackAction.action.performed += OnAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("ATAQUE!");
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        // Detecta enemigos dentro del radio de ataque
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            enemigo.GetComponent<EnemyHealth>().TakeDamage(damage, attackPoint);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
