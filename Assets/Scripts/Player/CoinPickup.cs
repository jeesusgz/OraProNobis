using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sumar monedas usando el CurrencyManager
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.AddMoney(value);

            // Actualizar UI
            if (CoinUI.Instance != null)
                CoinUI.Instance.PlayCoinEffect();

            // Guardar datos
            SaveSystem.Save(CurrencyManager.Instance.gameData);

            Destroy(gameObject);
        }
    }
}
