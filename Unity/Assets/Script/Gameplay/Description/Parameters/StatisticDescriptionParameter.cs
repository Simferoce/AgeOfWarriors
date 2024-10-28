using Game.Statistics;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDescriptionParameter : DescriptionParameter
    {
        [SerializeField] private string name;

        public override object GetValue(Entity source, Context context)
        {
            //if (source.TryGetCachedComponent<StatisticRegistry>(out StatisticRegistry statisticRegistry) && statisticRegistry.TryGetStatistic(name, out Statistic statistic))
            //    return statistic.GetDescription(context);

            return $"{{{name}}}";
        }
    }
}
