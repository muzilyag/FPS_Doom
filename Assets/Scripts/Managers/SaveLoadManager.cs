using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string accountId;
    public int maxWavesSurvived;
    public int zombieKilled;
    public float maxLiveTime;
    // Another data
}

[System.Serializable]
public class AllPlayersData
{
    public List<PlayerData> players = new List<PlayerData>();
}
public static class SaveLoadManager
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, "users.json");
    private static AllPlayersData _allData;
    public static string CurretnUser;
    static SaveLoadManager()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _allData = JsonUtility.FromJson<AllPlayersData>(json);
        }
        else
        {
            _allData = new AllPlayersData();
        }
    }

    public static PlayerData GetPlayerData(string accountId)
    {
        var player = _allData.players.Find(p => p.accountId == accountId);
        if (player == null)
        {
            player = new PlayerData { accountId = accountId };
            _allData.players.Add(player);
            SaveAllData();
        }
        return player;
    }

    public static void SaveAllData()
    {
        string json = JsonUtility.ToJson(_allData, true);
        File.WriteAllText(FilePath, json);
    }
}