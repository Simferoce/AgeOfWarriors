using System;

namespace Game
{
    public interface IStatisticProvider
    {
        public string StatisticProviderName { get; }

        public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic);
    }
}
