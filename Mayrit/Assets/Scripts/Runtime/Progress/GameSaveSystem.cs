using UnityEngine;
using System;

[Serializable]
public class PlayerProgressData
{
    public int StoredMilestoneIndex = -1; // Highest milestone index reached. -1 means no progress.
    public bool HasCompletedTutorial = false;
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

    public static void SaveMilestoneIdx(int milestoneIndex)
    {
        PlayerProgressData data = LoadAllData(); // Load existing progress to preserve tutorial completion status.
        data.StoredMilestoneIndex = milestoneIndex;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void SaveTutorial(bool hasCompletedTutorial)
    {
        PlayerProgressData data = LoadAllData(); // Load existing progress to preserve milestone index.
        data.HasCompletedTutorial = hasCompletedTutorial;

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