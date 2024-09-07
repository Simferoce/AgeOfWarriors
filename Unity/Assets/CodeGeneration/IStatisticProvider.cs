using System;
using System.Collections.Generic;

public interface IStatisticProvider
{
    public bool IsName(string name);
    public bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic, HashSet<object> visisted = null);
}