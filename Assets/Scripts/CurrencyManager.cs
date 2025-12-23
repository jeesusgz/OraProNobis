using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public GameData gameData;

    public event Action<int> OnMoneyChanged;

    [Header("Modo Presentación")]
    public bool modoPresentacion = false;
    public int dineroPresentacion = 99999;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveSystem.Load(gameData);  // Carga datos al iniciar

            //ACTIVAR DINERO DE PRESENTACIÓN
            if (modoPresentacion)
            {
                gameData.monedas = dineroPresentacion;
                SaveSystem.Save(gameData);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Actualizamos la UI al iniciar
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