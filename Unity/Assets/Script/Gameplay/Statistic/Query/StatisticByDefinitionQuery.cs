using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatisticByDefinitionQuery : StatisticQuery
{
    [SerializeReference, SubclassSelector] ReferenceProvider provider;
    [SerializeField] private StatisticDefinition statisticDefinition;

    public StatisticByDefinitionQuery()
    {

    }

    public StatisticByDefinitionQuery(ReferenceProvider provider, StatisticDefinition statisticDefinition)
    {
        this.provider = provider;
        this.statisticDefinition = statisticDefinition;
    }

    public override IEnumerable<Statistic> GetStatistics(IStatisticContext context)
    {
        if (provider == null)
            return Array.Empty<Statistic>();

        object resolved = provider.Resolve(context);
        if (!(resolved is IStatisticContext statisticContext))
            throw new Exception($"Expecting the result of {provider.GetType()} to be of type {nameof(IStatisticContext)}");

        return statisticContext.GetStatistic().Where(x => x.Definition == statisticDefinition);
    }
}