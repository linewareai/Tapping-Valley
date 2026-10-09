using UnityEngine;

namespace ValleyTapping.Economy
{
    public enum UpgradeKind { TapPower, PassiveIncome, PetBond }

    [CreateAssetMenu(menuName = "ValleyTapping/Upgrade Definition", fileName = "UpgradeDefinition")]
    public sealed class UpgradeDefinition : ScriptableObject
    {
        [SerializeField] private string upgradeId = "carrot";
        [SerializeField] private string displayName = "Huerto de zanahorias";
        [SerializeField, Min(1)] private long baseCost = 15;
        [SerializeField, Min(0)] private float costMultiplier = 1.15f;
        [SerializeField, Min(0)] private long valuePerLevel = 1;
        [SerializeField] private UpgradeKind kind = UpgradeKind.TapPower;

        public string UpgradeId { get { return upgradeId; } }
        public string DisplayName { get { return displayName; } }
        public long BaseCost { get { return baseCost; } }
        public float CostMultiplier { get { return costMultiplier; } }
        public long ValuePerLevel { get { return valuePerLevel; } }
        public UpgradeKind Kind { get { return kind; } }

        public long GetCost(int currentLevel)
        {
            int level = Mathf.Clamp(currentLevel, 0, 100);
            double cost = BaseCost * System.Math.Pow(System.Math.Max(1d, CostMultiplier), level);
            return (long)System.Math.Min(cost, long.MaxValue / 4d);
        }
    }
}
