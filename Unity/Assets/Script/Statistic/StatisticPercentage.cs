using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticPercentage : Statistic<float>
    {
        [SerializeField] private StatisticReference<float> reference;
        [SerializeField] private float percentage;

        public override string GetDescription(object caller)
        {
            return $"{percentage * 100} % of {reference.GetMapper(caller).GetDefinition().Title}";
        }

        public override float GetValue(object caller)
        {
            return reference.GetMapper(caller).GetValue() * percentage;
        }
    }
}
