using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDescriptionParameter : DescriptionParameter
    {
        [SerializeField] private string name;

        public override object GetValue(object context)
        {
            //if (context is Entity entity && entity.TryGetCachedComponent<StatisticIndex>(out StatisticIndex statisticIndex) && statisticIndex.TryGetStatisticByName(name, out Statistic statistic))
            //    return statistic.GetDescription(context);

            return $"{{{name}}}";
        }
    }
}
