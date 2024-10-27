using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CasterStatisticRatioAbilityStatisticValue : StatisticValue<float>
    {
        [SerializeField] private StatisticDefinition casterDefinition;
        [SerializeField, Range(0, 10)] private float ratio;

        public override float GetValue(Context context)
        {
            if (owner is not AbilityEntity ability)
                throw new Exception($"Excepting the type of {owner} to be of {nameof(AbilityEntity)}");

            return ability.Caster.Entity.GetCachedComponent<StatisticRegistry>().GetOrThrow<float>(casterDefinition, context) * ratio;
        }

        public override string GetDescription(Context context)
        {
            return $"<color=#{casterDefinition.ColorHex}>{ratio:0.0%}{casterDefinition.TextIcon}</color>";
        }
    }
}
