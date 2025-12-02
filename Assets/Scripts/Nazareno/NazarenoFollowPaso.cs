using UnityEngine;

public class NazarenoFollowPaso : MonoBehaviour
{
    public Transform paso;        // El paso
    public float offsetX;         // Distancia inicial en X
    public float speedMultiplier = 1f; // 1 = misma velocidad que el paso

    private Rigidbody2D rbPaso;
    private Rigidbody2D rbNazareno;

    void Start()
    {
        rbNazareno = GetComponent<Rigidbody2D>();
        rbPaso = paso.GetComponent<Rigidbody2D>();

        // Calculamos la distancia inicial una vez
        offsetX = transform.position.x - paso.position.x;
    }

    void FixedUpdate()
    {
        // El nazareno SIEMPRE mantiene la misma distancia en X al paso
        float targetX = paso.position.x + offsetX;

        // Copiamos la velocidad del paso
        rbNazareno.linearVelocity = new Vector2(rbPaso.linearVelocity.x * speedMultiplier, 0);

        // Corregimos poco a poco la distancia para que no se acumule error
        float correctionSpeed = 5f;
        float correctedX = Mathf.Lerp(transform.position.x, targetX, Time.fixedDeltaTime * correctionSpeed);

        rbNazareno.MovePosition(new Vector2(correctedX, transform.position.y));
    }
}