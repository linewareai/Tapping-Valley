using System;
using System.IO;
using UnityEngine;

namespace ValleyTapping
{
    public static class SettingsService
    {
        private const string FileName = "valleytapping-settings.json";
        private static string PathToSettings { get { return Path.Combine(Application.persistentDataPath, FileName); } }

        public static SettingsData Load()
        {
            try
            {
                if (!File.Exists(PathToSettings)) return new SettingsData();
                SettingsData data = JsonUtility.FromJson<SettingsData>(File.ReadAllText(PathToSettings));
                if (data == null) return new SettingsData();
                if (string.IsNullOrEmpty(data.language)) data.language = "es";
                data.qualityLevel = Mathf.Clamp(data.qualityLevel, 0, 2);
                return data;
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Could not load settings; defaults will be used. " + exception.Message);
                return new SettingsData();
            }
        }

        public static bool Save(SettingsData data)
        {
            if (data == null) return false;
            string temp = PathToSettings + ".tmp";
            string backup = PathToSettings + ".bak";
            try
            {
                data.qualityLevel = Mathf.Clamp(data.qualityLevel, 0, 2);
                File.WriteAllText(temp, JsonUtility.ToJson(data, true));
                if (File.Exists(PathToSettings))
                {
                    if (File.Exists(backup)) File.Delete(backup);
                    File.Move(PathToSettings, backup);
                }
                File.Move(temp, PathToSettings);
                if (File.Exists(backup)) File.Delete(backup);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError("Could not save settings: " + exception.Message);
                try
                {
                    if (File.Exists(temp)) File.Delete(temp);
                    if (!File.Exists(PathToSettings) && File.Exists(backup)) File.Move(backup, PathToSettings);
                }
                catch (Exception recoveryException)
                {
                    Debug.LogWarning("Settings recovery failed: " + recoveryException.Message);
                }
                return false;
            }
        }

        public static void Apply(SettingsData data)
        {
            if (data == null) return;
            AudioListener.pause = !data.soundEnabled && !data.musicEnabled;
            QualitySettings.SetQualityLevel(Mathf.Clamp(data.qualityLevel, 0, 2), true);
        }
    }
}
