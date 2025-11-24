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
            // Cargar monedas inmediatamente
            coins = PlayerPrefs.GetInt("Coins", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        Debug.Log("Monedas actuales: " + coins);
    }

    public void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }
}
