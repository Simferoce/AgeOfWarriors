using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticDynamicFloat : StatisticDynamic<float, MapperFloat> { }

    [Serializable]
    public class StatisticDynamic<T, M> : Statistic<T>
    {
        [SerializeField] private StatisticReference<T, M> reference;
        [SerializeField] private bool isPercentage;

        public override string GetDescription()
        {
            return "";
        }

        public override string GetDescriptionFormatted(object caller)
        {
            if (isPercentage == true)
                return $"{(float)(object)GetValue(caller) * 100}%";
            else
                return GetValue(caller).ToString();
        }

        public override T GetValue(object caller)
        {
            return reference.GetValueOrThrow(caller);
        }
    }
}
