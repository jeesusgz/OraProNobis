using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public GameData gameData;

    public event Action<int> OnMoneyChanged;

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

    private void Start()
    {
        OnMoneyChanged?.Invoke(gameData.monedas);
    }

    public bool TrySpend(int amount)
    {
        if (gameData.monedas >= amount)
        {
            gameData.monedas -= amount;
            OnMoneyChanged?.Invoke(gameData.monedas);
            SaveSystem.Save(gameData);
            return true;
        }

        return false;
    }

    public void AddMoney(int amount)
    {
        gameData.monedas += amount;
        OnMoneyChanged?.Invoke(gameData.monedas);
        SaveSystem.Save(gameData);
    }
}