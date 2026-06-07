using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgressData
{
    public int StoredMilestoneIndex = -1; // -1 means no progress.
    public bool HasCompletedTutorial = false;

    public List<int> FoundCollectiblesIDs = new();
}

public static class GameSaveSystem
{
    const string SaveKey = "Mayrit.PlayerProgress";

    public static PlayerProgressData LoadAllData()
    {
        // First launch or cleared save.
        if (!PlayerPrefs.HasKey(SaveKey))
            return new PlayerProgressData();

        string json = PlayerPrefs.GetString(SaveKey, string.Empty);
        // Defensive fallback in case key exists but value is empty.
        if (string.IsNullOrEmpty(json))
            return new PlayerProgressData();

        try
        {
            // Deserialize; null-coalesce for extra safety.
            return JsonUtility.FromJson<PlayerProgressData>(json) ?? new PlayerProgressData();
        }
        catch
        {
            // Corrupted/invalid JSON should never block gameplay.
            return new PlayerProgressData();
        }
    }

    public static int LoadMilestoneIdx()
    {
        PlayerProgressData data = LoadAllData();
        return data.StoredMilestoneIndex;
    }

    public static bool LoadTutorialCompletion()
    {
        PlayerProgressData data = LoadAllData();
        return data.HasCompletedTutorial;
    }

    public static List<int> LoadFoundCollectibles()
    {
        PlayerProgressData data = LoadAllData();
        return data.FoundCollectiblesIDs;
    }

    public static void SaveMilestoneIdx(int milestoneIndex)
    {
        PlayerProgressData data = LoadAllData();
        data.StoredMilestoneIndex = milestoneIndex;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void SaveTutorial(bool hasCompletedTutorial)
    {
        PlayerProgressData data = LoadAllData();
        data.HasCompletedTutorial = hasCompletedTutorial;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void SaveFoundCollectible(int id)
    {
        if (id < 0) return;

        PlayerProgressData data = LoadAllData();

        if (data.FoundCollectiblesIDs.Contains(id)) return;

        data.FoundCollectiblesIDs.Add(id);

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void ClearAllData()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

    public static void ResetTutorial()
    {
        PlayerProgressData data = LoadAllData();
        data.HasCompletedTutorial = false;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void ResetFoundCollectibles()
    {
        PlayerProgressData data = LoadAllData();
        data.FoundCollectiblesIDs.Clear();

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static bool IsThereStoredMilestoneIdx()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return false;

        string json = PlayerPrefs.GetString(SaveKey, string.Empty);
        if (string.IsNullOrEmpty(json))
            return false;

        try
        {
            PlayerProgressData data = JsonUtility.FromJson<PlayerProgressData>(json);
            return data != null && data.StoredMilestoneIndex >= 0;
        }
        catch
        {
            return false;
        }
    }
}