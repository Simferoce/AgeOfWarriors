using System;

namespace Game
{
    public interface IStatisticProviderOld
    {
        public string StatisticProviderName { get; }

        public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic);
        public T GetStatisticOrDefault<T>(string path)
        {
            return TryGetStatistic<T>(path, out T statistic) ? statistic : default;
        }
        public T GetStatisticOrDefault<T>(string path, T defaultValue)
        {
            return TryGetStatistic<T>(path, out T statistic) ? statistic : defaultValue;
        }
    }
}
