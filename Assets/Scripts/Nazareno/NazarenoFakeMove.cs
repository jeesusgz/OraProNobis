using UnityEngine;

public class NazarenoFakeMove : MonoBehaviour
{
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        float offset = Mathf.Sin(timer * 15f) * 0.0005f;
        transform.position += new Vector3(offset, 0, 0);
    }
}
