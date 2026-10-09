using System;
using System.Collections.Generic;
using ValleyTapping;
using UnityEngine;

namespace ValleyTapping.Quests
{
    public sealed class QuestService
    {
        private readonly Dictionary<string, QuestDefinition> definitions = new Dictionary<string, QuestDefinition>(StringComparer.Ordinal);
        private readonly SaveData save;

        public QuestService(IEnumerable<QuestDefinition> quests, SaveData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            save = data;
            if (save.quests == null) save.quests = new List<QuestProgressData>();
            if (quests != null)
            {
                foreach (QuestDefinition quest in quests)
                    if (quest != null && !string.IsNullOrEmpty(quest.QuestId))
                        definitions[quest.QuestId] = quest;
            }
        }

        public bool AddProgress(string questId, int amount)
        {
            QuestDefinition definition;
            if (amount <= 0 || !definitions.TryGetValue(questId ?? string.Empty, out definition)) return false;
            QuestProgressData progress = GetOrCreate(questId);
            progress.progress = (int)Math.Min(definition.Target, (long)progress.progress + amount);
            return true;
        }

        public bool TryClaim(string questId, out string message)
        {
            QuestDefinition definition;
            if (!definitions.TryGetValue(questId ?? string.Empty, out definition))
            {
                message = "Misión no encontrada.";
                return false;
            }
            QuestProgressData progress = GetOrCreate(questId);
            if (progress.claimed || progress.progress < definition.Target)
            {
                message = "Completa la misión antes de reclamar la recompensa.";
                return false;
            }
            if (save.coins > long.MaxValue - definition.RewardCoins)
            {
                message = "El saldo ha alcanzado el límite seguro.";
                return false;
            }
            save.coins += definition.RewardCoins;
            save.lifetimeCoinsEarned = Math.Min(long.MaxValue, save.lifetimeCoinsEarned + definition.RewardCoins);
            progress.claimed = true;
            message = "Recompensa recibida: " + definition.RewardCoins + " monedas.";
            return true;
        }

        private QuestProgressData GetOrCreate(string id)
        {
            for (int i = 0; i < save.quests.Count; i++)
                if (save.quests[i] != null && save.quests[i].questId == id) return save.quests[i];
            QuestProgressData created = new QuestProgressData { questId = id, progress = 0, claimed = false };
            save.quests.Add(created);
            return created;
        }
    }
}
