using System;

namespace ValleyTapping
{
    [Serializable]
    public sealed class SaveData
    {
        public int schemaVersion = 1;
        public long coins;
        public int tapPowerLevel;
        public long lifetimeCoinsEarned;
    }
}
