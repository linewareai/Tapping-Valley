using System;
using System.Collections.Generic;
using ValleyTapping.Farm;
using ValleyTapping.Quests;

namespace ValleyTapping
{
    [Serializable]
    public sealed class SaveData
    {
        public int schemaVersion = 2;
        public long coins;
        public int tapPowerLevel;
        public long lifetimeCoinsEarned;

        // Farm state uses timestamps so crop growth can continue while the app is closed.
        public FarmPlotData[] farmPlots = new FarmPlotData[6];

        // Stable IDs are stored instead of Unity object references.
        public List<string> ownedPets = new List<string>();
        public List<int> petCarePoints = new List<int>();
        public string activePetId;
        public List<QuestProgressData> quests = new List<QuestProgressData>();

        public SaveData()
        {
            for (int i = 0; i < farmPlots.Length; i++)
                farmPlots[i] = new FarmPlotData { plotIndex = i };
        }
    }
}
