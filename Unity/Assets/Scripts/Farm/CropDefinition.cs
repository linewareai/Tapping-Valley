using System;
using UnityEngine;

namespace ValleyTapping.Farm
{
    [CreateAssetMenu(menuName = "ValleyTapping/Crop Definition", fileName = "CropDefinition")]
    public sealed class CropDefinition : ScriptableObject
    {
        [SerializeField] private string cropId = "carrot";
        [SerializeField] private string displayName = "Zanahoria";
        [SerializeField, Min(1)] private int seedCost = 5;
        [SerializeField, Min(1)] private int growSeconds = 60;
        [SerializeField, Min(0)] private int harvestReward = 12;
        [SerializeField] private Sprite seedSprite;
        [SerializeField] private Sprite growingSprite;
        [SerializeField] private Sprite readySprite;

        public string CropId { get { return cropId; } }
        public string DisplayName { get { return displayName; } }
        public int SeedCost { get { return seedCost; } }
        public int GrowSeconds { get { return growSeconds; } }
        public int HarvestReward { get { return harvestReward; } }
        public Sprite SeedSprite { get { return seedSprite; } }
        public Sprite GrowingSprite { get { return growingSprite; } }
        public Sprite ReadySprite { get { return readySprite; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(cropId))
                cropId = name.Trim().ToLowerInvariant().Replace(" ", "-");
            seedCost = Math.Max(1, seedCost);
            growSeconds = Math.Max(1, growSeconds);
            harvestReward = Math.Max(0, harvestReward);
        }
    }
}
