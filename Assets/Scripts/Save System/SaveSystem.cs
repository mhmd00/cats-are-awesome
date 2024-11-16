using UnityEngine;
using System.IO;
public class SaveSystem : MonoBehaviour
{
    private const string SaveFileName = "gameData.json";

    public static void SaveGameData(GameData data)
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
    }

    public static GameData LoadGameData()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        return new GameData();
    }
}