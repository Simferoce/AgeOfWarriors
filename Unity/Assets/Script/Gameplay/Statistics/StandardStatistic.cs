using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class StandardStatistic : Statistic<float, StandardStatisticDefinition>
    {
        [SerializeReference, SubclassSelector] protected StatisticValue value;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            value.Initialize(owner);
        }

        protected override float GetValue(Context context)
        {
            StatisticIndex index = owner.GetCachedComponent<StatisticIndex>();

            float baseValue = value.GetValue<float>(context);
            float flatValue = index.Sum(definition.Flat, context);
            float percentageValue = index.Sum(definition.Percentage, context);
            float multiplierValue = index.Multiply(definition.Multiplier, context);
            float maximumValue = index.Maximum(definition.Maximum, context);
            float minimumValue = index.Minimum(definition.Minimum, context);

            return Mathf.Clamp((baseValue + flatValue) * (1 + percentageValue) * multiplierValue, minimumValue, maximumValue);
        }
    }
}
