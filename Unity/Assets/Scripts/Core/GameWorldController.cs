using System;
using UnityEngine;
using UnityEngine.UI;
using ValleyTapping.Achievements;
using ValleyTapping.Economy;
using ValleyTapping.Farm;
using ValleyTapping.Pets;
using ValleyTapping.Quests;

namespace ValleyTapping
{
    /// <summary>
    /// Optional scene-level controller that connects the domain services to Unity UI.
    /// Assign ScriptableObject definitions and UI references in the Inspector.
    /// </summary>
    public sealed class GameWorldController : MonoBehaviour
    {
        [Header("Definitions")]
        [SerializeField] private CropDefinition[] crops;
        [SerializeField] private PetDefinition[] pets;
        [SerializeField] private UpgradeDefinition[] upgrades;
        [SerializeField] private QuestDefinition[] quests;
        [SerializeField] private AchievementDefinition[] achievements;

        [Header("Core UI")]
        [SerializeField] private Text coinsText;
        [SerializeField] private Text gemsText;
        [SerializeField] private Text statusText;
        [SerializeField] private Text tapPowerText;
        [SerializeField] private Text playerLevelText;

        [Header("Farm UI")]
        [SerializeField] private Text[] plotLabels;
        [SerializeField] private string defaultCropId = "carrot";

        [Header("Pet UI")]
        [SerializeField] private Text activePetText;
        [SerializeField] private Text petNeedsText;

        [Header("Progress UI")]
        [SerializeField] private Text questSummaryText;
        [SerializeField] private Text achievementSummaryText;

        private SaveData data;
        private FarmService farm;
        private PetService petService;
        private UpgradeService upgradeService;
        private QuestService questService;
        private AchievementService achievementService;

        private void Awake()
        {
            data = SaveSystem.Load();
            farm = new FarmService(crops, data);
            petService = new PetService(pets, data);
            upgradeService = new UpgradeService(upgrades, data);
            questService = new QuestService(quests, data);
            achievementService = new AchievementService(achievements, data);
            achievementService.Unlocked += OnAchievementUnlocked;
            ApplyOfflineProgress();
            PetNeedsService.Advance(data, 0L);
            RefreshAll();
        }

        public void Tap()
        {
            long reward = 1L + Math.Max(0, data.tapPowerLevel);
            if (data.coins > long.MaxValue / 4L - reward)
            {
                SetStatus("Has alcanzado el límite seguro de monedas.");
                return;
            }
            data.coins += reward;
            data.lifetimeCoinsEarned = Math.Min(long.MaxValue / 4L, data.lifetimeCoinsEarned + reward);
            data.totalTaps = data.totalTaps == int.MaxValue ? int.MaxValue : data.totalTaps + 1;
            data.playerLevel = Math.Max(1, 1 + (int)Math.Sqrt(Math.Max(0L, data.lifetimeCoinsEarned) / 100L));
            if (quests != null)
            {
                foreach (QuestDefinition quest in quests)
                    if (quest != null && quest.QuestId == "first-tap")
                        questService.AddProgress(quest.QuestId, 1);
            }
            achievementService.Evaluate(data.totalTaps);
            Persist();
            RefreshAll();
            SetStatus("+" + reward + " monedas");
        }

        public void BuyUpgrade(string upgradeId)
        {
            string message;
            if (upgradeService.TryBuy(upgradeId, out message))
            {
                data.tapPowerLevel = upgradeService.GetLevel("tap-power");
                Persist();
            }
            SetStatus(message);
            RefreshAll();
        }

        public void PlantDefaultCrop(int plotIndex)
        {
            string message;
            if (farm.TryPlant(plotIndex, defaultCropId, Now(), out message))
                Persist();
            SetStatus(message);
            RefreshAll();
        }

        public void Harvest(int plotIndex)
        {
            long reward;
            string message;
            if (farm.TryHarvest(plotIndex, Now(), out reward, out message))
            {
                if (quests != null)
                    foreach (QuestDefinition quest in quests)
                        if (quest != null && quest.QuestId == "first-harvest")
                            questService.AddProgress(quest.QuestId, 1);
                achievementService.Evaluate(data.totalTaps);
                Persist();
            }
            SetStatus(message);
            RefreshAll();
        }

        public void AdoptPet(string petId)
        {
            string message;
            if (petService.TryAdopt(petId, out message))
                Persist();
            SetStatus(message);
            RefreshAll();
        }

        public void SetActivePet(string petId)
        {
            string message;
            if (petService.TrySetActive(petId, out message))
                Persist();
            SetStatus(message);
            RefreshAll();
        }

        public void FeedActivePet()
        {
            if (PetNeedsService.Feed(data, 5)) { Persist(); SetStatus("Tu mascota ha comido."); }
            else SetStatus("Necesitas una mascota activa y 5 monedas.");
            RefreshAll();
        }

        public void RestActivePet()
        {
            if (PetNeedsService.Rest(data)) { Persist(); SetStatus("Tu mascota ha descansado."); }
            else SetStatus("Primero adopta una mascota.");
            RefreshAll();
        }

        public void CareForActivePet()
        {
            string message;
            if (string.IsNullOrEmpty(data.activePetId))
                message = "Primero adopta una mascota.";
            else if (petService.TryCare(data.activePetId, out message))
            {
                data.petHappiness = Mathf.Min(100f, data.petHappiness + 5f);
                data.petBond = data.petBond == int.MaxValue ? int.MaxValue : data.petBond + 1;
                Persist();
            }
            SetStatus(message);
            RefreshAll();
        }

        public void ClaimQuest(string questId)
        {
            string message;
            if (questService.TryClaim(questId, out message))
                Persist();
            SetStatus(message);
            RefreshAll();
        }

        private void ApplyOfflineProgress()
        {
            long now = Now();
            if (data.lastSeenUnixSeconds > 0L)
            {
                // Passive income is only granted when a passive-income upgrade exists.
                long passiveRate = 0L;
                if (upgrades != null)
                {
                    foreach (UpgradeDefinition definition in upgrades)
                    {
                        if (definition != null && definition.Kind == UpgradeKind.PassiveIncome)
                            passiveRate += upgradeService.GetLevel(definition.UpgradeId) * definition.ValuePerLevel;
                    }
                }
                long earned = OfflineProgressService.CalculatePassiveEarnings(
                    data.lastSeenUnixSeconds, now, passiveRate, 0d);
                if (earned > 0L && data.coins <= long.MaxValue / 4L - earned)
                {
                    data.coins += earned;
                    data.lifetimeCoinsEarned = Math.Min(long.MaxValue / 4L, data.lifetimeCoinsEarned + earned);
                }
            }
            data.lastSeenUnixSeconds = now;
            Persist();
        }

        private void RefreshAll()
        {
            if (coinsText != null) coinsText.text = "Monedas: " + data.coins;
            if (gemsText != null) gemsText.text = "Gemas: " + data.gems;
            data.tapPowerLevel = upgradeService.GetLevel("tap-power");
            if (tapPowerText != null) tapPowerText.text = "Por toque: " + (1 + Math.Max(0, data.tapPowerLevel));
            if (playerLevelText != null) playerLevelText.text = "Nivel: " + data.playerLevel;

            if (plotLabels != null)
            {
                for (int i = 0; i < plotLabels.Length; i++)
                {
                    if (plotLabels[i] == null) continue;
                    FarmPlotData plot = farm.GetPlot(i);
                    if (plot == null || plot.IsEmpty) plotLabels[i].text = "Parcela " + (i + 1) + ": libre";
                    else if (plot.IsReady(Now())) plotLabels[i].text = "Parcela " + (i + 1) + ": cosechar";
                    else plotLabels[i].text = "Parcela " + (i + 1) + ": creciendo";
                }
            }

            if (activePetText != null)
                activePetText.text = string.IsNullOrEmpty(data.activePetId) ? "Mascota activa: ninguna" : "Mascota activa: " + data.activePetId;
            if (petNeedsText != null)
                petNeedsText.text = "Hambre " + Mathf.RoundToInt(data.petHunger) + "% · Felicidad " + Mathf.RoundToInt(data.petHappiness) + "% · Energía " + Mathf.RoundToInt(data.petEnergy) + "%";

            if (questSummaryText != null)
            {
                int completed = 0;
                int claimed = 0;
                if (data.quests != null)
                    foreach (QuestProgressData progress in data.quests)
                        if (progress != null) { if (progress.progress > 0) completed++; if (progress.claimed) claimed++; }
                questSummaryText.text = "Misiones con progreso: " + completed + " · Reclamadas: " + claimed;
            }
            if (achievementSummaryText != null)
                achievementSummaryText.text = "Logros desbloqueados: " + data.unlockedAchievements.Count;
        }

        private void Persist()
        {
            data.lastSeenUnixSeconds = Now();
            if (!SaveSystem.Save(data))
                SetStatus("No se pudo guardar la partida. Comprueba el espacio disponible.");
        }

        private void OnAchievementUnlocked(AchievementDefinition definition)
        {
            SetStatus("¡Logro desbloqueado: " + definition.DisplayName + "!");
        }

        private void SetStatus(string message)
        {
            if (statusText != null) statusText.text = message;
        }

        private static long Now()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused && data != null) Persist();
        }

        private void OnApplicationQuit()
        {
            if (data != null) Persist();
        }

        private void OnDestroy()
        {
            if (achievementService != null) achievementService.Unlocked -= OnAchievementUnlocked;
        }
    }
}
