using System;
using System.IO;
using UnityEngine;

namespace ValleyTapping
{
    public static class SaveSystem
    {
        private const string FileName = "valleytapping-save.json";
        private const int CurrentSchemaVersion = 1;

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

                // Future schema migrations belong here, before updating schemaVersion.
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
            if (data == null)
                return false;

            string temporaryPath = SavePath + ".tmp";

            try
            {
                data.schemaVersion = CurrentSchemaVersion;
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(temporaryPath, json);

                if (File.Exists(SavePath))
                    File.Delete(SavePath);

                File.Move(temporaryPath, SavePath);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError("Could not save ValleyTapping progress: " + exception.Message);
                try
                {
                    if (File.Exists(temporaryPath))
                        File.Delete(temporaryPath);
                }
                catch (Exception cleanupException)
                {
                    Debug.LogWarning("Could not clean temporary save: " + cleanupException.Message);
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
