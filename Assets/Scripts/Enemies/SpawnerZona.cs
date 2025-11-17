using UnityEngine;

public class SpawnerZona : MonoBehaviour
{
    [Header("Prefabs de enemigos")]
    public GameObject esqueletoPrefab;
    public GameObject fantasmaPrefab;
    public GameObject gatoPrefab;

    [Header("Zona de spawn")]
    public BoxCollider2D zonaSpawn;

    [Header("Referencias de la escena")]
    public Transform player;
    public Transform paso;

    [Header("Spawn config")]
    public float tiempoEntreSpawn = 3f;
    public int maxEnemigos = 5;

    [Range(0, 100)] public int probEsqueleto = 60;
    [Range(0, 100)] public int probFantasma = 30;
    [Range(0, 100)] public int probGato = 10;

    private float timerSpawn = 0f;
    private float tiempoTotal = 0f;
    public float intervaloDificultad = 20f;
    private int nivel = 1;

    void Update()
    {
        tiempoTotal += Time.deltaTime;
        timerSpawn += Time.deltaTime;

        if (timerSpawn >= tiempoEntreSpawn)
        {
            timerSpawn = 0f;
            SpawnEnemigo();
        }

        if (tiempoTotal >= nivel * intervaloDificultad)
        {
            SubirDificultad();
        }
    }

    void SpawnEnemigo()
    {
        // Limite de enemigos
        int actuales = GameObject.FindGameObjectsWithTag("Enemigo").Length;
        if (actuales >= maxEnemigos) return;

        int rng = Random.Range(0, 100);
        GameObject prefab = ElegirPrefab(rng);
        if (prefab == null) return;

        Vector3 spawnPos = ObtenerPosicionDentroZona();

        GameObject enemigo = Instantiate(prefab, spawnPos, Quaternion.identity);
        enemigo.tag = "Enemigo";

        // Asignar referencias al clone
        var ai = enemigo.GetComponent<SkeletonController>();
        if (ai != null)
        {
            ai.player = player;
            ai.paso = paso;
        }

        var ghost = enemigo.GetComponentInChildren<GhostController>();
        if (ghost != null)
        {
            ghost.player = player;
            ghost.paso = paso;
        }
    }

    GameObject ElegirPrefab(int rng)
    {
        int acumulado = probEsqueleto;
        if (rng < acumulado) return esqueletoPrefab;

        acumulado += probFantasma;
        if (rng < acumulado) return fantasmaPrefab;

        return gatoPrefab;
    }

    Vector3 ObtenerPosicionDentroZona()
    {
        if (zonaSpawn == null)
        {
            Debug.LogWarning("[SpawnerZona] No hay zona asignada. Usando posición del spawner.");
            return transform.position;
        }

        Vector2 min = zonaSpawn.bounds.min;
        Vector2 max = zonaSpawn.bounds.max;

        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);

        return new Vector3(x, y, 0f);
    }

    void SubirDificultad()
    {
        nivel++;
        maxEnemigos += 2;
        tiempoEntreSpawn = Mathf.Max(0.7f, tiempoEntreSpawn - 0.3f);

        if (probEsqueleto > 30) probEsqueleto -= 5;
        probFantasma += 3;
        probGato += 2;

        // normalizar probabilidades
        int total = probEsqueleto + probFantasma + probGato;
        float factor = 100f / total;
        probEsqueleto = Mathf.RoundToInt(probEsqueleto * factor);
        probFantasma = Mathf.RoundToInt(probFantasma * factor);
        probGato = Mathf.RoundToInt(probGato * factor);

        Debug.Log("🔺 Nivel " + nivel + " | maxEnemigos: " + maxEnemigos + " | tiempoEntreSpawn: " + tiempoEntreSpawn);
    }
}
