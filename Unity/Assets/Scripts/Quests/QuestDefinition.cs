using UnityEngine;

namespace ValleyTapping.Quests
{
    [CreateAssetMenu(menuName = "ValleyTapping/Quest Definition", fileName = "QuestDefinition")]
    public sealed class QuestDefinition : ScriptableObject
    {
        [SerializeField] private string questId = "first-harvest";
        [SerializeField] private string displayName = "Primera cosecha";
        [SerializeField, Min(1)] private int target = 1;
        [SerializeField, Min(0)] private int rewardCoins = 20;
        public string QuestId { get { return questId; } }
        public string DisplayName { get { return displayName; } }
        public int Target { get { return target; } }
        public int RewardCoins { get { return rewardCoins; } }
    }
}
