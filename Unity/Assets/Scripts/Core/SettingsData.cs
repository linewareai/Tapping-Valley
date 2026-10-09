using System;

namespace ValleyTapping
{
    [Serializable]
    public sealed class SettingsData
    {
        public bool musicEnabled = true;
        public bool soundEnabled = true;
        public bool vibrationEnabled = true;
        public string language = "es";
        public int qualityLevel = 1;
    }
}
