using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string name;

        public Statistic<T> Resolve(Entity entity)
        {
            if (entity.GetCachedComponent<StatisticRegistry>().TryGetStatistic<T>(name, out Statistic<T> value))
                return value;

            return null;
        }
    }
}
