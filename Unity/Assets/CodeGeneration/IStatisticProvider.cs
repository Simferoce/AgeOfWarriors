using System;
using System.Collections.Generic;

public interface IStatisticProvider
{
    public class StatisticProviderResolution
    {
        public IStatisticProvider Provider { get; set; }
        public string Path { get; set; }
        public bool UseStatistic { get; set; }

        public StatisticProviderResolution(IStatisticProvider provider, string path, bool useStatistic)
        {
            Provider = provider;
            Path = path;
            UseStatistic = useStatistic;
        }
    }

    public bool IsName(string name);
    public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic);
    public IEnumerator<StatisticProviderResolution> GetStatisticProvider();
}