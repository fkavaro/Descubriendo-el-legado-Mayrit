using UnityEngine;
using System;

[Serializable]
public class PlayerProgressData
{
    // Highest milestone the player has reached at least once.
    // The game restores this value on startup to load the corresponding scene/state.
    public int HighestCompletedMilestoneIndex = 0;
}

public static class GameSaveSystem
{
    // Single PlayerPrefs entry that stores the whole progress payload as JSON.
    const string SaveKey = "Mayrit.PlayerProgress.v1";

    /// <summary>
    /// Loads player progress from PlayerPrefs. If no valid save exists, returns a new Player
    /// ProgressData instance with default values.
    /// </summary>
    public static PlayerProgressData Load()
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

    /// <summary>
    /// Saves player progress to PlayerPrefs as JSON. Overwrites any existing save.
    /// </summary>
    public static void Save(int highestCompletedMilestoneIndex)
    {
        PlayerProgressData data = new()
        {
            HighestCompletedMilestoneIndex = Mathf.Max(0, highestCompletedMilestoneIndex)
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        // Force write to disk now (important before app quit/crash scenarios).
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }
}