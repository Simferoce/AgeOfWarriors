using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class SerializeStatisticFloat : SerializeStatistic<float> { }

    [Serializable]
    public class SerializeStatistic<T> : Statistic
    {
        [SerializeField] private StatisticDefinition definition;
        [SerializeField] private T value;

        public override StatisticDefinition GetDefinition(object context)
        {
            return definition;
        }

        public override U GetValue<U>(object context)
        {
            return StatisticUtility.ConvertGeneric<U, T>(value);
        }
    }
}
