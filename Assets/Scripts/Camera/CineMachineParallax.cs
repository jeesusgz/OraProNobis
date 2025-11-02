using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class CinemachineParallax : MonoBehaviour
{
    [SerializeField] private Camera cam; // Cámara principal (puede ser la de Cinemachine)
    [SerializeField, Range(0f, 1f)] private float parallaxEffect = 0.5f;
    private float startX;
    private float spriteWidth;
    private Transform camTransform;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;

        camTransform = cam.transform;
        startX = transform.position.x;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
    }

    private void LateUpdate()
    {
        if (camTransform == null)
            return;

        float distanceMoved = camTransform.position.x * parallaxEffect;
        transform.position = new Vector3(startX + distanceMoved, transform.position.y, transform.position.z);
    }

    private void OnValidate()
    {
        if (cam == null)
            cam = Camera.main;
    }
}
