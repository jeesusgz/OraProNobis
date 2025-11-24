using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance;

    public int coins = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Monedas actuales: " + coins);

        // Guardado persistente
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }
}
