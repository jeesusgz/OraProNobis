using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Configuración del seguimiento")]
    public Transform player; // Referencia al jugador
    public float lookAheadDistance = 2f; // Qué tanto se adelanta la cámara
    public float lookSmoothTime = 0.3f; // Suavidad del movimiento

    private CinemachineCamera cineCam;
    private Vector3 currentVelocity;
    private float lookAheadDirection = 0f;

    private void Start()
    {
        cineCam = GetComponent<CinemachineCamera>();

        if (player == null)
        {
            Debug.LogWarning("CameraFollow2D: no se asignó el jugador, buscando automáticamente...");
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void LateUpdate()
    {
        if (player == null || cineCam == null)
            return;

        // Detectar la dirección del jugador (izquierda/derecha)
        float targetDirection = Mathf.Sign(player.localScale.x);
        lookAheadDirection = Mathf.SmoothDamp(lookAheadDirection, targetDirection, ref currentVelocity.x, lookSmoothTime);

        // Calcular desplazamiento dinámico
        Vector3 offset = new Vector3(lookAheadDirection * lookAheadDistance, 0, 0);

        // Aplicar el offset al Follow Offset del Cinemachine Follow
        var follow = cineCam.GetComponent<CinemachineFollow>();
        if (follow != null)
        {
            Vector3 currentOffset = follow.FollowOffset;
            Vector3 targetOffset = new Vector3(offset.x, currentOffset.y, currentOffset.z);
            follow.FollowOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * 5f);
        }
    }
}
