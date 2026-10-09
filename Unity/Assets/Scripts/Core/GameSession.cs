using System;
using UnityEngine;
using UnityEngine.UI;

namespace ValleyTapping
{
    public sealed class GameSession : MonoBehaviour
    {
        private const long BaseUpgradeCost = 25L;
        private const long MaximumSafeBalance = long.MaxValue / 4L;

        [Header("UI (assign in Inspector)")]
        [SerializeField] private Text coinsText;
        [SerializeField] private Text tapPowerText;
        [SerializeField] private Text upgradeCostText;
        [SerializeField] private Text statusText;
        [SerializeField] private Button upgradeButton;

        private SaveData saveData;

        private long TapPower
        {
            get { return 1L + saveData.tapPowerLevel; }
        }

        private long UpgradeCost
        {
            get
            {
                // Cost grows by 50% per level, with a bounded level to prevent overflow.
                double cost = BaseUpgradeCost * Math.Pow(1.5d, Math.Min(saveData.tapPowerLevel, 40));
                return Math.Max(BaseUpgradeCost, (long)Math.Min(cost, MaximumSafeBalance));
            }
        }

        private void Awake()
        {
            saveData = SaveSystem.Load();
            RefreshUI();
        }

        public void Tap()
        {
            if (saveData == null)
                return;

            long reward = TapPower;
            if (saveData.coins > MaximumSafeBalance - reward)
            {
                SetStatus("¡Has alcanzado el límite de monedas de esta versión!");
                return;
            }

            saveData.coins += reward;
            saveData.lifetimeCoinsEarned = Math.Min(
                MaximumSafeBalance,
                saveData.lifetimeCoinsEarned + reward);

            SaveSystem.Save(saveData);
            RefreshUI();
            SetStatus("+" + reward + " monedas");
        }

        public void BuyUpgrade()
        {
            if (saveData == null)
                return;

            long cost = UpgradeCost;
            if (saveData.coins < cost)
            {
                SetStatus("Te faltan " + (cost - saveData.coins) + " monedas.");
                return;
            }

            if (saveData.tapPowerLevel >= 40)
            {
                SetStatus("¡Mejora máxima alcanzada en el prototipo!");
                return;
            }

            saveData.coins -= cost;
            saveData.tapPowerLevel++;

            if (!SaveSystem.Save(saveData))
                SetStatus("No se pudo confirmar el guardado. Revisa el espacio disponible.");
            else
                SetStatus("¡Mejora comprada! Cada toque vale " + TapPower + ".");

            RefreshUI();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused && saveData != null)
                SaveSystem.Save(saveData);
        }

        private void OnApplicationQuit()
        {
            if (saveData != null)
                SaveSystem.Save(saveData);
        }

        private void RefreshUI()
        {
            if (saveData == null)
                return;

            if (coinsText != null)
                coinsText.text = "Monedas: " + saveData.coins;

            if (tapPowerText != null)
                tapPowerText.text = "Por toque: " + TapPower;

            if (upgradeCostText != null)
                upgradeCostText.text = "Mejorar toque: " + UpgradeCost + " monedas";

            if (upgradeButton != null)
                upgradeButton.interactable = saveData.tapPowerLevel < 40;
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
    }
}
