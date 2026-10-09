using System;

namespace ValleyTapping.Farm
{
    [Serializable]
    public sealed class FarmPlotData
    {
        public int plotIndex;
        public string cropId;
        public long plantedAtUnixSeconds;
        public long readyAtUnixSeconds;

        public bool IsEmpty { get { return string.IsNullOrEmpty(cropId); } }

        public bool IsReady(long nowUnixSeconds)
        {
            return !IsEmpty && nowUnixSeconds >= readyAtUnixSeconds;
        }

        public void Clear()
        {
            cropId = null;
            plantedAtUnixSeconds = 0L;
            readyAtUnixSeconds = 0L;
        }
    }
}
