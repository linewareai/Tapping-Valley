using UnityEngine;

namespace ValleyTapping.Achievements
{
    [CreateAssetMenu(menuName = "ValleyTapping/Achievement Definition", fileName = "AchievementDefinition")]
    public sealed class AchievementDefinition : ScriptableObject
    {
        [SerializeField] private string achievementId = "first-tap";
        [SerializeField] private string displayName = "Primer toque";
        [SerializeField, Min(0)] private long requiredLifetimeCoins;
        [SerializeField, Min(0)] private int requiredTapCount;
        public string AchievementId { get { return achievementId; } }
        public string DisplayName { get { return displayName; } }
        public long RequiredLifetimeCoins { get { return requiredLifetimeCoins; } }
        public int RequiredTapCount { get { return requiredTapCount; } }
    }
}
