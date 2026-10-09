using System;
using System.Collections.Generic;
using ValleyTapping;

namespace ValleyTapping.Economy
{
    public sealed class UpgradeService
    {
        private readonly SaveData save;
        private readonly Dictionary<string, UpgradeDefinition> definitions = new Dictionary<string, UpgradeDefinition>(StringComparer.Ordinal);

        public UpgradeService(IEnumerable<UpgradeDefinition> upgrades, SaveData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            save = data;
            if (save.upgrades == null) save.upgrades = new List<UpgradeProgressData>();
            if (upgrades == null) return;
            foreach (UpgradeDefinition definition in upgrades)
                if (definition != null && !string.IsNullOrEmpty(definition.UpgradeId))
                    definitions[definition.UpgradeId] = definition;
        }

        public int GetLevel(string id)
        {
            UpgradeProgressData entry = Find(id);
            return entry == null ? 0 : Math.Max(0, entry.level);
        }

        public long GetCost(string id)
        {
            UpgradeDefinition definition;
            return definitions.TryGetValue(id ?? string.Empty, out definition)
                ? definition.GetCost(GetLevel(id)) : -1L;
        }

        public bool TryBuy(string id, out string message)
        {
            UpgradeDefinition definition;
            if (!definitions.TryGetValue(id ?? string.Empty, out definition))
            {
                message = "Mejora no encontrada.";
                return false;
            }
            UpgradeProgressData progress = Find(id);
            if (progress != null && progress.level >= 100)
            {
                message = "Has alcanzado el nivel máximo del prototipo.";
                return false;
            }
            long cost = definition.GetCost(progress == null ? 0 : progress.level);
            if (save.coins < cost)
            {
                message = "No tienes suficientes monedas.";
                return false;
            }
            save.coins -= cost;
            if (progress == null)
            {
                progress = new UpgradeProgressData { upgradeId = id, level = 0 };
                save.upgrades.Add(progress);
            }
            progress.level++;
            message = "Mejora comprada: " + definition.DisplayName + ".";
            return true;
        }

        private UpgradeProgressData Find(string id)
        {
            for (int i = 0; i < save.upgrades.Count; i++)
                if (save.upgrades[i] != null && save.upgrades[i].upgradeId == id) return save.upgrades[i];
            return null;
        }
    }
}
