using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance;

    public Image coinIcon;
    public TextMeshProUGUI coinsText;
    public AudioClip coinSound;
    public float popScale = 1.5f;
    public float popDuration = 0.2f;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Suscribirse a eventos del CurrencyManager
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnMoneyChanged += UpdateUI;
        }

        UpdateUI(CurrencyManager.Instance.gameData.monedas);
    }

    public void UpdateUI(int value)
    {
        coinsText.text = value.ToString();
    }

    public void PlayCoinEffect()
    {
        if (coinIcon != null)
        {
            coinIcon.transform.DOKill();
            coinIcon.transform.localScale = Vector3.one;
            coinIcon.transform.DOScale(popScale, popDuration)
                .SetLoops(2, LoopType.Yoyo);
        }

        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }
}
