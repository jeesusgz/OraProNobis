using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sumamos monedas
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.AddCoins(value);

            // Efecto sonoro opcional
            // AudioSource.PlayClipAtPoint(coinSound, transform.position);

            Destroy(gameObject);
        }
    }
}
