using System;

public interface IStatisticProvider
{
    public bool IsName(string name);
    public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic);
}