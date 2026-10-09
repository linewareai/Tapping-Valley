using System;
using System.Collections.Generic;
using ValleyTapping;
using UnityEngine;

namespace ValleyTapping.Farm
{
    public sealed class FarmService
    {
        private readonly Dictionary<string, CropDefinition> crops;
        private readonly SaveData save;

        public FarmService(IEnumerable<CropDefinition> definitions, SaveData saveData)
        {
            if (saveData == null) throw new ArgumentNullException("saveData");
            save = saveData;
            crops = new Dictionary<string, CropDefinition>(StringComparer.Ordinal);

            if (definitions == null) return;
            foreach (CropDefinition definition in definitions)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.CropId)) continue;
                crops[definition.CropId] = definition;
            }

            if (save.farmPlots == null) save.farmPlots = new FarmPlotData[6];
            for (int i = 0; i < save.farmPlots.Length; i++)
            {
                if (save.farmPlots[i] == null)
                    save.farmPlots[i] = new FarmPlotData { plotIndex = i };
            }
        }

        public bool TryPlant(int plotIndex, string cropId, long nowUnixSeconds, out string message)
        {
            FarmPlotData plot;
            CropDefinition crop;
            if (!TryGetPlot(plotIndex, out plot) || !crops.TryGetValue(cropId ?? string.Empty, out crop))
            {
                message = "Parcela o cultivo no válido.";
                return false;
            }
            if (!plot.IsEmpty)
            {
                message = "Esta parcela ya está ocupada.";
                return false;
            }
            if (save.coins < crop.SeedCost)
            {
                message = "No tienes suficientes monedas para esas semillas.";
                return false;
            }

            save.coins -= crop.SeedCost;
            plot.cropId = crop.CropId;
            plot.plantedAtUnixSeconds = nowUnixSeconds;
            plot.readyAtUnixSeconds = nowUnixSeconds + crop.GrowSeconds;
            message = crop.DisplayName + " plantada.";
            return true;
        }

        public bool TryHarvest(int plotIndex, long nowUnixSeconds, out long reward, out string message)
        {
            reward = 0L;
            FarmPlotData plot;
            if (!TryGetPlot(plotIndex, out plot) || plot.IsEmpty)
            {
                message = "No hay nada plantado en esa parcela.";
                return false;
            }
            if (!plot.IsReady(nowUnixSeconds))
            {
                message = "El cultivo todavía está creciendo.";
                return false;
            }

            CropDefinition crop;
            if (!crops.TryGetValue(plot.cropId, out crop))
            {
                message = "No se encuentra la definición del cultivo. La parcela se conserva.";
                return false;
            }

            reward = crop.HarvestReward;
            if (reward > 0L && save.coins > long.MaxValue - reward)
            {
                message = "El saldo ha alcanzado el límite seguro.";
                reward = 0L;
                return false;
            }

            save.coins += reward;
            save.lifetimeCoinsEarned = Math.Min(long.MaxValue, save.lifetimeCoinsEarned + reward);
            plot.Clear();
            message = "¡Cosecha recogida! +" + reward + " monedas.";
            return true;
        }

        public FarmPlotData GetPlot(int plotIndex)
        {
            FarmPlotData plot;
            return TryGetPlot(plotIndex, out plot) ? plot : null;
        }

        private bool TryGetPlot(int index, out FarmPlotData plot)
        {
            plot = null;
            if (save.farmPlots == null || index < 0 || index >= save.farmPlots.Length) return false;
            plot = save.farmPlots[index];
            if (plot == null)
            {
                plot = new FarmPlotData { plotIndex = index };
                save.farmPlots[index] = plot;
            }
            return true;
        }
    }
}
