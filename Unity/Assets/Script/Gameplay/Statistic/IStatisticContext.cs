using System;
using System.Collections.Generic;

public interface IStatisticContext
{
    public bool IsName(ReadOnlySpan<char> name);
    public IEnumerable<Statistic> GetStatistic();
}