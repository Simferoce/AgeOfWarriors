using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDynamicFloat : StatisticDynamic<float> { }

    [Serializable]
    public class StatisticDynamic<T> : Statistic<T>
    {
        [SerializeField] private StatisticReference<T> reference;

        public override string GetDescription()
        {
            return "";
        }

        public override string GetDescriptionFormatted(object caller)
        {
            return GetValue(caller).ToString();
        }

        public override T GetValue(object caller)
        {
            return reference.GetValueOrThrow(caller);
        }
    }
}
