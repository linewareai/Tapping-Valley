using System;

namespace ValleyTapping
{
    public static class OfflineProgressService
    {
        public const int MaximumOfflineSeconds = 7200;

        public static long CalculatePassiveEarnings(long lastSeenUnixSeconds, long nowUnixSeconds, long coinsPerSecond, double bonusPercent)
        {
            if (coinsPerSecond <= 0L || nowUnixSeconds <= lastSeenUnixSeconds) return 0L;
            long elapsed = Math.Min(MaximumOfflineSeconds, nowUnixSeconds - lastSeenUnixSeconds);
            double multiplier = 1d + Math.Max(0d, Math.Min(10000d, bonusPercent)) / 100d;
            double reward = elapsed * (double)coinsPerSecond * multiplier;
            return reward >= long.MaxValue ? long.MaxValue : Math.Max(0L, (long)Math.Floor(reward));
        }
    }
}
