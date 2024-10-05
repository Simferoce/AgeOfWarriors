using System.Collections.Generic;

public interface IStatisticContext
{
    public IEnumerable<Statistic> GetStatistic();
}