using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticInheritFloat : StatisticInherit<float>, IStatisticFloat
    {
        public IStatisticFloat Clone()
        {
            StatisticInheritFloat statisticInherit = new StatisticInheritFloat();
            statisticInherit.reference = reference.Clone();
            return statisticInherit;
        }
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
