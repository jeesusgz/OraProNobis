using UnityEngine;

public static class SaveSystem
{
    private const string KEY = "MY_GAME_DATA_V1";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(new SerializableData(data));
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public static void Load(GameData data)
    {
        if (!PlayerPrefs.HasKey(KEY)) return;
        string json = PlayerPrefs.GetString(KEY);
        var s = JsonUtility.FromJson<SerializableData>(json);
        s.ApplyTo(data);
    }

    [System.Serializable]
    private class SerializableData
    {
        public int monedas;
        public int vidaPasoNivel;
        public int estaminaPasoNivel;
        public int vidaJugadorNivel;
        public int dañoJugadorNivel;
        public int cantidadNazarenos;

        public SerializableData(GameData d)
        {
            monedas = d.monedas;
            vidaPasoNivel = d.vidaPasoNivel;
            estaminaPasoNivel = d.estaminaPasoNivel;
            vidaJugadorNivel = d.vidaJugadorNivel;
            dañoJugadorNivel = d.dañoJugadorNivel;
            cantidadNazarenos = d.cantidadNazarenos;
        }

        public void ApplyTo(GameData d)
        {
            d.monedas = monedas;
            d.vidaPasoNivel = vidaPasoNivel;
            d.estaminaPasoNivel = estaminaPasoNivel;
            d.vidaJugadorNivel = vidaJugadorNivel;
            d.dañoJugadorNivel = dañoJugadorNivel;
            d.cantidadNazarenos = cantidadNazarenos;
        }
    }
}
