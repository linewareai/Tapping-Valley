using System;

namespace ValleyTapping.Quests
{
    [Serializable]
    public sealed class QuestProgressData
    {
        public string questId;
        public int progress;
        public bool claimed;
    }
}
