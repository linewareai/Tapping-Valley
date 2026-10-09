using System;
using System.Collections.Generic;
using ValleyTapping;

namespace ValleyTapping.Achievements
{
    public sealed class AchievementService
    {
        private readonly SaveData save;
        private readonly List<AchievementDefinition> definitions;
        public event Action<AchievementDefinition> Unlocked;

        public AchievementService(IEnumerable<AchievementDefinition> source, SaveData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            save = data;
            if (save.unlockedAchievements == null) save.unlockedAchievements = new List<string>();
            definitions = source == null ? new List<AchievementDefinition>() : new List<AchievementDefinition>(source);
        }

        public void Evaluate(int totalTaps)
        {
            foreach (AchievementDefinition definition in definitions)
            {
                if (definition == null || string.IsNullOrEmpty(definition.AchievementId)) continue;
                if (save.unlockedAchievements.Contains(definition.AchievementId)) continue;
                if (save.lifetimeCoinsEarned < definition.RequiredLifetimeCoins || totalTaps < definition.RequiredTapCount) continue;
                save.unlockedAchievements.Add(definition.AchievementId);
                Action<AchievementDefinition> handler = Unlocked;
                if (handler != null) handler(definition);
            }
        }
    }
}
