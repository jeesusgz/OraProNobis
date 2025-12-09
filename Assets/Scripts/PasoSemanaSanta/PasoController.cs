using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PasoController : MonoBehaviour
{
    [Header("Datos Persistentes del Juego")]
    public GameData gameData;

    [Header("Configuración base")]
    public float distanciaActivacion = 2f;
    public Transform jugador;
    public bool jugadorCerca = false;
    public InputActionReference levantarAction;

    private Animator animator;
    private bool animado = false;
    private bool levantado = false;

    [Header("Movimiento hacia la capilla")]
    public float moveSpeed = 1f;
    public Transform targetPoint;
    private Rigidbody2D rb;

    private bool puedeMoverse = false;
    private bool entrando = false;
    public float arriveThreshold = 0.3f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Collider2D mainCollider;

    [Header("Estamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSecond = 20f;
    public float staminaRecoveryPerSecond = 10f;
    public float fullDrainPenaltyTime = 2f;
    public float lowStaminaMultiplier = 0.5f;

    [Header("UI del Paso")]
    public UnityEngine.UI.Image staminaBar;

    private bool fullDrainPenaltyActive = false;
    private float penaltyTimer = 0f;

    // =========================
    //    NAZARENOS / SLOTS
    // =========================
    [Header("Nazarenos")]
    public GameObject nazarenoPrefab;
    public Transform destinoCapilla;
    public int maxSlotsDelante = 2;
    public int maxSlotsDetras = 2;

    [Header("Colliders de Spawn")]
    public BoxCollider2D colliderDelante;
    public BoxCollider2D colliderDetras;
    private float distanciaSeguridad = 0.10f;

    [HideInInspector] public NazarenoController[] slotsDelante;
    [HideInInspector] public NazarenoController[] slotsDetras;

    private void Awake()
    {
        slotsDelante = new NazarenoController[maxSlotsDelante];
        slotsDetras = new NazarenoController[maxSlotsDetras];
    }

    private void OnEnable()
    {
        if (levantarAction != null)
        {
            levantarAction.action.Enable();
            levantarAction.action.performed += OnLevantar;
        }
    }

    private void OnDisable()
    {
        if (levantarAction != null)
        {
            levantarAction.action.performed -= OnLevantar;
            levantarAction.action.Disable();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider2D>();

        ApplyUpgradesFromGameData();
        currentStamina = maxStamina;
    }

    private void ApplyUpgradesFromGameData()
    {
        if (gameData.vidaPasoNivel > 0)
            maxStamina += gameData.vidaPasoNivel * 5f;

        if (gameData.estaminaPasoNivel > 0)
            maxStamina += gameData.estaminaPasoNivel * 20f;

        if (gameData.velocidadPasoNivel > 0)
        {
            moveSpeed += gameData.velocidadPasoNivel * 0.5f;
            if (moveSpeed > 3f) moveSpeed = 3f;
        }
    }

    private void OnLevantar(InputAction.CallbackContext context)
    {
        if (!jugadorCerca || animado || fullDrainPenaltyActive || currentStamina <= 0f)
            return;

        animado = true;

        if (!levantado)
        {
            animator.SetTrigger("Levanta");
            levantado = true;
            puedeMoverse = false;
            Invoke(nameof(ActivarMovimiento), 2f);
        }
        else
        {
            animator.SetTrigger("Arria");
            levantado = false;
            puedeMoverse = false;
        }

        Invoke(nameof(FinAnimacion), 3f);
    }

    void Update()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (fullDrainPenaltyActive)
        {
            penaltyTimer -= Time.deltaTime;
            if (penaltyTimer <= 0f) fullDrainPenaltyActive = false;
        }

        if (!animado && !fullDrainPenaltyActive)
            RecoverStamina();

        if (!entrando && targetPoint != null)
        {
            float dist = Mathf.Abs(transform.position.x - targetPoint.position.x);
            if (dist <= arriveThreshold)
                EntrarCapilla();
        }

        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }

    void FixedUpdate()
    {
        if (levantado && puedeMoverse && targetPoint != null && !entrando)
        {
            float speedMultiplier = currentStamina / maxStamina < 0.2f ? lowStaminaMultiplier : 1f;

            Vector2 next = Vector2.MoveTowards(
                rb.position,
                new Vector2(targetPoint.position.x, rb.position.y),
                moveSpeed * speedMultiplier * Time.fixedDeltaTime
            );

            rb.MovePosition(next);
            animator.SetBool("Andando", true);

            currentStamina -= staminaDrainPerSecond * Time.fixedDeltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                fullDrainPenaltyActive = true;
                penaltyTimer = fullDrainPenaltyTime;
                puedeMoverse = false;

                if (levantado)
                {
                    animator.SetTrigger("Arria");
                    levantado = false;
                    animado = true;
                    Invoke(nameof(FinAnimacion), 3f);
                }
            }
        }
    }

    void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryPerSecond * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }

    private void EntrarCapilla()
    {
        if (entrando) return;
        entrando = true;

        puedeMoverse = false;
        levantado = false;
        animator.SetBool("Andando", false);

        if (mainCollider != null)
            mainCollider.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(FadeAndDestroy(1.5f));
    }

    private void ActivarMovimiento() => puedeMoverse = true;
    public void FinAnimacion() => animado = false;

    private IEnumerator FadeAndDestroy(float duration)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            foreach (var sr in sprites)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    sr.color = new Color(c.r, c.g, c.b, alpha);
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // =========================
    // NAZARENOS
    // =========================
    public bool ComprarNazareno(bool delante)
    {
        NazarenoController[] slotArray = delante ? slotsDelante : slotsDetras;

        for (int i = 0; i < slotArray.Length; i++)
        {
            if (slotArray[i] == null)
            {
                SpawnNazareno(i, delante);
                return true;
            }
        }

        return false;
    }

    private void SpawnNazareno(int slotIndex, bool delante)
    {
        NazarenoController[] slotArray = delante ? slotsDelante : slotsDetras;

        GameObject n = Instantiate(nazarenoPrefab);
        NazarenoController nc = n.GetComponent<NazarenoController>();

        nc.paso = this;
        nc.destinoCapilla = destinoCapilla;
        nc.delante = delante;
        nc.lider = slotIndex == 0 ? null : slotArray[slotIndex - 1];

        slotArray[slotIndex] = nc;

        BoxCollider2D box = delante ? colliderDelante : colliderDetras;
        if (box != null)
        {
            float spacing = nc.GetComponent<Collider2D>().bounds.size.x + distanciaSeguridad;
            float spawnX = delante
                ? box.bounds.min.x + spacing / 2 + slotIndex * spacing
                : box.bounds.max.x - spacing / 2 - slotIndex * spacing;

            float spawnY = box.bounds.min.y;
            n.transform.position = new Vector3(spawnX, spawnY, transform.position.z);
        }
        else
        {
            n.transform.position = transform.position;
        }
    }

    public void NotifyNazarenoDeath(NazarenoController naz)
    {
        for (int i = 0; i < slotsDelante.Length; i++)
            if (slotsDelante[i] == naz) slotsDelante[i] = null;

        for (int i = 0; i < slotsDetras.Length; i++)
            if (slotsDetras[i] == naz) slotsDetras[i] = null;
    }

    public bool Animado => animado;
    public bool FullDrainPenaltyActive => fullDrainPenaltyActive;
    public float CurrentStamina => currentStamina;
    public bool Levantado => levantado;
    public bool PuedeMoverse => puedeMoverse;
}