using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    public HealthSystem playerHealth;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public GameObject heartPrefab;
    public Transform heartsContainer;

    private List<Image> hearts = new List<Image>();

    void OnEnable()
    {
        if (playerHealth != null)
        {
            HealthSystem.OnMaxHealthChanged += UpdateMaxHealth;
            HealthSystem.OnPlayerDamaged += UpdateHeartsFromHealthSystem;
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            HealthSystem.OnMaxHealthChanged -= UpdateMaxHealth;
            HealthSystem.OnPlayerDamaged -= UpdateHeartsFromHealthSystem;
        }
    }

    // ✅ Nuevo método público para inicializar corazones correctamente
    public void InicializarHearts()
    {
        if (playerHealth == null) return;

        InitializeHearts(playerHealth.maxHealth);
        UpdateHeartsFromHealthSystem();
    }

    void InitializeHearts(int maxHealth)
    {
        foreach (Transform child in heartsContainer)
            Destroy(child.gameObject);
        hearts.Clear();

        int totalHearts = Mathf.CeilToInt(maxHealth / 2f);

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(h.GetComponent<Image>());
        }
    }

    void UpdateMaxHealth(int newMaxHealth)
    {
        int totalHearts = Mathf.CeilToInt(newMaxHealth / 2f);

        if (totalHearts > hearts.Count)
        {
            int heartsToAdd = totalHearts - hearts.Count;
            for (int i = 0; i < heartsToAdd; i++)
            {
                GameObject h = Instantiate(heartPrefab, heartsContainer);
                hearts.Add(h.GetComponent<Image>());
            }
        }

        UpdateHeartsFromHealthSystem();
    }

    void UpdateHeartsFromHealthSystem()
    {
        if (playerHealth == null) return;

        int health = playerHealth.currentHealth;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (health >= 2)
            {
                hearts[i].sprite = fullHeart;
                health -= 2;
            }
            else if (health == 1)
            {
                hearts[i].sprite = halfHeart;
                health -= 1;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
