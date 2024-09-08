using System;
using System.Collections.Generic;

public interface IStatisticProvider
{
    public bool IsName(string name);
    public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic);
    public IEnumerator<(IStatisticProvider provider, string path)> GetStatisticProvider();
}