using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class ReferenceByNameStatistic : Statistic
    {
        [SerializeField] private string referenceName;

        public override string GetDescription(object context)
        {
            if (context is IEntity entity && entity.TryGetCachedComponent<StatisticIndex>(out StatisticIndex index) && index.TryGetStatisticByName(referenceName, out Statistic statistic))
                return statistic.GetDescription(context);

            return $"{{{referenceName}}}";
        }

        public override T GetValue<T>(object context)
        {
            if (context is IEntity entity && entity.TryGetCachedComponent<StatisticIndex>(out StatisticIndex index) && index.TryGetStatisticByName(referenceName, out Statistic statistic))
                return statistic.GetValue<T>(context);

            Debug.LogError($"Unable to resolve the statistic {referenceName} from {context}", context as UnityEngine.Object);
            return default(T);
        }
    }
}
