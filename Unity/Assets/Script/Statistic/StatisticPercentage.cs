using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticPercentage : Statistic<float>, IStatisticFloat
    {
        [SerializeField] private StatisticDefinition definition;
        [SerializeField] private StatisticReference<float> reference;
        [SerializeField] private float percentage;

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return definition;
        }

        public override float GetValue(StatisticContext context)
        {
            return reference.Get(context).GetValue(context) * percentage;
        }

        public IStatisticFloat Clone()
        {
            StatisticPercentage statisticPercentage = new StatisticPercentage();
            statisticPercentage.reference = reference.Clone();
            statisticPercentage.percentage = percentage;
            statisticPercentage.definition = definition;
            return statisticPercentage;
        }
    }
}
