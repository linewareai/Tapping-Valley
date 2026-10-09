using System;
using System.Collections.Generic;
using ValleyTapping.Economy;
using ValleyTapping.Farm;
using ValleyTapping.Quests;

namespace ValleyTapping
{
    [Serializable]
    public sealed class SaveData
    {
        public int schemaVersion = 3;
        public long coins;
        public long gems;
        public long lifetimeCoinsEarned;
        public int tapPowerLevel;
        public int totalTaps;
        public int playerLevel = 1;
        public long lastSeenUnixSeconds;

        public FarmPlotData[] farmPlots = new FarmPlotData[8];
        public List<InventoryEntry> inventory = new List<InventoryEntry>();
        public List<UpgradeProgressData> upgrades = new List<UpgradeProgressData>();

        public List<string> ownedPets = new List<string>();
        public List<int> petCarePoints = new List<int>();
        public string activePetId;
        public float petHunger = 82f;
        public float petHappiness = 78f;
        public float petEnergy = 90f;
        public int petBond;
        public List<QuestProgressData> quests = new List<QuestProgressData>();
        public List<string> unlockedAchievements = new List<string>();

        public SaveData()
        {
            for (int i = 0; i < farmPlots.Length; i++)
                farmPlots[i] = new FarmPlotData { plotIndex = i };
        }
    }
}
