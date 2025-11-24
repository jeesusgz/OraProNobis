using UnityEngine;

public class Coin : MonoBehaviour
{
    public float fallSpeed = 5f;
    public LayerMask groundMask;

    private bool isLanded = false;

    void Update()
    {
        if (!isLanded)
        {
            // Mover hacia abajo
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Raycast desde la base de la moneda hacia abajo
            float rayLength = 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundMask);

            if (hit.collider != null)
            {
                // Ajustar posición justo sobre el suelo
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.05f, transform.position.z);
                isLanded = true; // bloquea movimiento
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Para ver el raycast en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
    }
}

