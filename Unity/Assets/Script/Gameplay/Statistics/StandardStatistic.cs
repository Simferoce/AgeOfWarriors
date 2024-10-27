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
            StatisticRegistry registry = owner.GetCachedComponent<StatisticRegistry>();

            float baseValue = value.GetValue<float>(context);
            float flatValue = registry.Sum(definition.Flat, context);
            float percentageValue = registry.Sum(definition.Percentage, context);
            float multiplierValue = registry.Multiply(definition.Multiplier, context);
            float maximumValue = registry.Maximum(definition.Maximum, context);
            float minimumValue = registry.Minimum(definition.Minimum, context);

            return Mathf.Clamp((baseValue + flatValue) * (1 + percentageValue) * multiplierValue, minimumValue, maximumValue);
        }

        public override string GetDescription(Context context)
        {
            string valueDescrition = value.GetDescription(context);
            if (!string.IsNullOrEmpty(valueDescrition))
                return $"{valueDescrition}<color=#{definition.ColorHex}>({GetValue(context)})</color>";

            return $"<color=#{definition.ColorHex}>({GetValue(context)})</color>";
        }
    }
}
