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
            SaveSystem.Load(gameData);  // Carga datos al iniciar
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Llamamos a la UI al iniciar
        OnMoneyChanged?.Invoke(gameData.monedas);
    }

    public void AddMoney(int amount)
    {
        gameData.monedas += amount;

        OnMoneyChanged?.Invoke(gameData.monedas);

        SaveSystem.Save(gameData);
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
}