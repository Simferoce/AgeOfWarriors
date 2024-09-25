using System;
using System.Collections.Generic;

[Serializable]
public abstract class StatisticQuery
{
    public abstract IEnumerable<Statistic> GetStatistics(IStatisticContext context);
}
