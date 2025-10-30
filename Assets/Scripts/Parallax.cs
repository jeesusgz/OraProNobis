using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class Parallax: MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField, Range(0f, 1f)] private float parallaxX = 0.5f;
    [SerializeField, Range(0f, 1f)] private float parallaxY = 0f;
    [SerializeField] private bool loopX = true;
    [SerializeField] private bool loopY = false;

    private float spriteWidth;
    private float spriteHeight;
    private Vector3 startPos;
    private Transform camTransform;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;

        camTransform = cam.transform;
        startPos = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
        spriteHeight = sr.bounds.size.y;
    }

    private void LateUpdate()
    {
        if (camTransform == null) return;

        // Movimiento parallax en X y Y
        float moveX = (camTransform.position.x * parallaxX);
        float moveY = (camTransform.position.y * parallaxY);

        transform.position = new Vector3(startPos.x + moveX, startPos.y + moveY, startPos.z);

        // --- Loop horizontal ---
        if (loopX)
        {
            float tempX = camTransform.position.x * (1 - parallaxX);
            if (tempX > startPos.x + spriteWidth)
                startPos.x += spriteWidth;
            else if (tempX < startPos.x - spriteWidth)
                startPos.x -= spriteWidth;
        }

        // --- Loop vertical (si lo activas) ---
        if (loopY)
        {
            float tempY = camTransform.position.y * (1 - parallaxY);
            if (tempY > startPos.y + spriteHeight)
                startPos.y += spriteHeight;
            else if (tempY < startPos.y - spriteHeight)
                startPos.y -= spriteHeight;
        }
    }
}
