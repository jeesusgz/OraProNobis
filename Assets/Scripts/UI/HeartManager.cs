using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public GameObject heartPrefab;
    public Transform heartsContainer;

    private List<Image> hearts = new List<Image>();

    void Start()
    {
        currentHealth = maxHealth;
        CreateHearts();
        UpdateHearts();
    }

    void CreateHearts()
    {
        int totalHearts = maxHealth / 2;

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(h.GetComponent<Image>());
        }
    }

    public void UpdateHearts()
    {
        int health = currentHealth;

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

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHearts();
    }
}
