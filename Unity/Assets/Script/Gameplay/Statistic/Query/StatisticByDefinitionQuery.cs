using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatisticByDefinitionQuery : StatisticQuery
{
    [SerializeReference, SubclassSelector] StatisticProvider provider;
    [SerializeField] private StatisticDefinition statisticDefinition;

    public override IEnumerable<Statistic> GetStatistics(IStatisticContext context)
    {
        return provider.Resolve(context).GetStatistic().Where(x => x.Definition == statisticDefinition);
    }
}