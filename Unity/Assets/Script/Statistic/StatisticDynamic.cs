using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDynamicFloatCharacter : StatisticDynamic<float, Character>, IStatisticFloat
    {
        public StatisticDynamicFloatCharacter(Func<Character, StatisticContext, float> func) : base(func)
        {
        }
    }

    [Serializable]
    public class StatisticDynamic<T, O> : Statistic<T>
    {
        [SerializeField] protected StatisticDefinition definition;
        protected O owner;
        protected Func<O, StatisticContext, T> func;

        public void Initialize(O owner)
        {
            this.owner = owner;
        }

        public StatisticDynamic(Func<O, StatisticContext, T> func)
        {
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
