using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticStandard : StatisticSerializedIdentity
    {
        [SerializeReference, SubclassSelector] private Statistic baseStatistic;
        [SerializeReference, SubclassSelector] private Statistic flatStatistic;
        [SerializeReference, SubclassSelector] private Statistic percentageStatistic;

        public StatisticStandard()
        {

        }

        public StatisticStandard(string name, string definitionId, Statistic baseStatistic, Statistic flatStatistic, Statistic percentageStatistic)
            : base(name, definitionId)
        {
            this.baseStatistic = baseStatistic;
            this.flatStatistic = flatStatistic;
            this.percentageStatistic = percentageStatistic;
        }

        public override void Initialize(IStatisticContext context)
        {
            base.Initialize(context);

            baseStatistic?.Initialize(context);
            flatStatistic?.Initialize(context);
            percentageStatistic?.Initialize(context);
        }

        public override bool TryGetValue<T>(out T value)
        {
            float baseValue = baseStatistic?.GetValueOrDefault<float>() ?? 0f;
            float flatValue = flatStatistic?.GetValueOrDefault<float>() ?? 0f;
            float percentageValue = percentageStatistic?.GetValueOrDefault<float>() ?? 0f;

            value = StatisticUtility.ConvertGeneric<T, float>((baseValue + flatValue) * (1 + percentageValue));
            return true;
        }
    }
}