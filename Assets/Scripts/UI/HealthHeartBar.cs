using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject HeartPrefab;
    public HealthSystem playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable()
    {
        HealthSystem.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        HealthSystem.OnPlayerDamaged -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        float maxHealthRemainer = playerHealth.maxHealth % 2;
        int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainer);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = Mathf.Clamp(playerHealth.currentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(HeartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartCompnent = newHeart.GetComponent<HealthHeart>();
        heartCompnent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartCompnent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
}
