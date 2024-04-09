using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticInheritFloat : StatisticInherit<float>, IStatisticFloat
    {
    }

    [Serializable]
    public class StatisticInherit<T> : Statistic<T>
    {
        [SerializeField] protected StatisticReference<T> reference;

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return reference.Get(context).GetDefinition(context);
        }

        public override T GetValue(StatisticContext context)
        {
            return reference.Get(context).GetValue(context);
        }
    }
}
