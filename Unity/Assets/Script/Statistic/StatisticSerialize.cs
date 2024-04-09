using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticSerializeFloat : StatisticSerialize<float>, IStatisticFloat
    {
        public StatisticSerializeFloat() : base(0)
        {
        }

        public StatisticSerializeFloat(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class StatisticSerialize<T> : Statistic<T>
    {
        [SerializeField] protected StatisticDefinition definition;
        [SerializeField] protected T value;

        public StatisticSerialize(T value)
        {
            this.value = value;
        }

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return definition;
        }

        public override T GetValue(StatisticContext context)
        {
            return value;
        }
    }
}
