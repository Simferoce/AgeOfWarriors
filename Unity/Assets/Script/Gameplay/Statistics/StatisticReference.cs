using System;
using System.Linq;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string name;

        public Statistic<T> Resolve(Entity entity)
        {
            return entity.GetCachedComponent<StatisticRegistry>().Statistics.OfType<Statistic<T>>().FirstOrDefault(x => x.Name == name);
        }
    }
}
