using System;
using System.IO;
using UnityEngine;

namespace ValleyTapping
{
    public static class SaveSystem
    {
        private const string FileName = "valleytapping-save.json";
        private const int CurrentSchemaVersion = 3;

        private static string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, FileName); }
        }

        public static SaveData Load()
        {
            if (!File.Exists(SavePath))
                return CreateNewSave();

            try
            {
                string json = File.ReadAllText(SavePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null || data.schemaVersion > CurrentSchemaVersion || data.schemaVersion < 1)
                {
                    Debug.LogWarning("Save data is invalid or from an unsupported version. Starting a new save.");
                    return CreateNewSave();
                }

                // Schema v1 only contained the tap-clicker economy. Preserve it while adding new systems.
                if (data.schemaVersion == 1)
                {
                    data.farmPlots = new ValleyTapping.Farm.FarmPlotData[6];
                    for (int i = 0; i < data.farmPlots.Length; i++)
                        data.farmPlots[i] = new ValleyTapping.Farm.FarmPlotData { plotIndex = i };
                    data.ownedPets = new System.Collections.Generic.List<string>();
                    data.petCarePoints = new System.Collections.Generic.List<int>();
                    data.quests = new System.Collections.Generic.List<ValleyTapping.Quests.QuestProgressData>();
                    data.activePetId = null;
                }

                if (data.farmPlots == null)
                    data.farmPlots = new ValleyTapping.Farm.FarmPlotData[8];
                for (int i = 0; i < data.farmPlots.Length; i++)
                    if (data.farmPlots[i] == null)
                        data.farmPlots[i] = new ValleyTapping.Farm.FarmPlotData { plotIndex = i };

                if (data.inventory == null) data.inventory = new System.Collections.Generic.List<InventoryEntry>();
                if (data.upgrades == null) data.upgrades = new System.Collections.Generic.List<ValleyTapping.Economy.UpgradeProgressData>();
                if (data.unlockedAchievements == null) data.unlockedAchievements = new System.Collections.Generic.List<string>();
                if (data.ownedPets == null) data.ownedPets = new System.Collections.Generic.List<string>();
                if (data.petCarePoints == null) data.petCarePoints = new System.Collections.Generic.List<int>();
                if (data.quests == null) data.quests = new System.Collections.Generic.List<ValleyTapping.Quests.QuestProgressData>();

                data.gems = Math.Max(0L, data.gems);
                data.totalTaps = Math.Max(0, data.totalTaps);
                data.playerLevel = Math.Max(1, data.playerLevel);
                data.petHunger = UnityEngine.Mathf.Clamp(data.petHunger, 0f, 100f);
                data.petHappiness = UnityEngine.Mathf.Clamp(data.petHappiness, 0f, 100f);
                data.petEnergy = UnityEngine.Mathf.Clamp(data.petEnergy, 0f, 100f);
                data.schemaVersion = CurrentSchemaVersion;
                data.coins = Math.Max(0L, data.coins);
                data.tapPowerLevel = Math.Max(0, data.tapPowerLevel);
                data.lifetimeCoinsEarned = Math.Max(0L, data.lifetimeCoinsEarned);
                return data;
            }
            catch (Exception exception)
            {
                Debug.LogError("Could not load ValleyTapping save: " + exception.Message);
                return CreateNewSave();
            }
        }

        public static bool Save(SaveData data)
        {
            if (data == null) return false;

            string temporaryPath = SavePath + ".tmp";
            try
            {
                data.schemaVersion = CurrentSchemaVersion;
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(temporaryPath, json);

                // File.Replace is not consistently available on every Unity target.
                // Keep a backup during replacement so a failed move does not destroy the last save.
                string backupPath = SavePath + ".bak";
                if (File.Exists(SavePath))
                {
                    if (File.Exists(backupPath)) File.Delete(backupPath);
                    File.Move(SavePath, backupPath);
                }

                File.Move(temporaryPath, SavePath);
                if (File.Exists(backupPath)) File.Delete(backupPath);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError("Could not save ValleyTapping progress: " + exception.Message);
                try
                {
                    if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
                    string backupPath = SavePath + ".bak";
                    if (!File.Exists(SavePath) && File.Exists(backupPath))
                        File.Move(backupPath, SavePath);
                }
                catch (Exception cleanupException)
                {
                    Debug.LogWarning("Save recovery cleanup failed: " + cleanupException.Message);
                }
                return false;
            }
        }

        private static SaveData CreateNewSave()
        {
            return new SaveData
            {
                schemaVersion = CurrentSchemaVersion,
                coins = 0L,
                tapPowerLevel = 0,
                lifetimeCoinsEarned = 0L
            };
        }
    }
}
