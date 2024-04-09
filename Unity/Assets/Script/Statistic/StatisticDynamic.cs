using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDynamicFloat : StatisticDynamic<float>, IStatisticFloat { }

    [Serializable]
    public class StatisticDynamic<T> : Statistic<T>, IStatisticOwn
    {
        [SerializeField] protected StatisticDefinition definition;

        public Func<object, StatisticContext, T> Function { get; set; }
        public object Owner { get; set; }

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return definition;
        }

        public override T GetValue(StatisticContext context)
        {
            return Owner != null ? Function.Invoke(Owner, context) : default(T);
        }
    }
}
