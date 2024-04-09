using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticPercentage : Statistic<float>
    {
        [SerializeField] private StatisticReference<float> reference;
        [SerializeField] private float percentage;

        public override string GetDescription()
        {
            return $"{percentage * 100} % of {reference.Path}";
        }

        public override float GetValue(object caller)
        {
            return reference.GetValue(caller) * percentage;
        }
    }
}
