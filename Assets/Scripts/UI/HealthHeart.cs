using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullheart;
    public Sprite halfheart;
    public Sprite emptyheart;

    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyheart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfheart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullheart;
                break;
        }

    }
}

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2,
    
}
