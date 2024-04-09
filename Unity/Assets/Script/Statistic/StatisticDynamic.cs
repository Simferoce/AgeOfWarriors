using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDynamicFloat : StatisticDynamic<float>, IStatisticFloat { }

    [Serializable]
    public class StatisticDynamic<T> : Statistic<T>
    {
        [SerializeField] protected StatisticDefinition definition;

        protected Func<object, StatisticContext, T> func;

        public void Initialize(object owner, Func<object, StatisticContext, T> func)
        {
            base.Initialize(owner);
            this.func = func;
        }

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return definition;
        }

        public override T GetValue(StatisticContext context)
        {
            return owner != null ? func.Invoke(owner, context) : default(T);
        }
    }
}
