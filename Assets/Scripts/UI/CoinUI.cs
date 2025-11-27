using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Image coinIcon;
    public TextMeshProUGUI coinsText; // Cambiar a Text si usas UI Text
    public AudioClip coinSound;
    public float popScale = 1.5f;
    public float popDuration = 0.2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (CurrencySystem.Instance != null)
        {
            coinsText.text = CurrencySystem.Instance.coins.ToString();
        }
    }

    // Llamar al recoger moneda
    public void PlayCoinEffect()
    {
        // Animación pop
        if (coinIcon != null)
        {
            coinIcon.transform.DOKill();
            coinIcon.transform.localScale = Vector3.one;
            coinIcon.transform.DOScale(popScale, popDuration).SetLoops(2, LoopType.Yoyo);
        }

        // Sonido
        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }

        UpdateUI();
    }
}
