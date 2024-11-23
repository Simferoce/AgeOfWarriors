using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CasterStatisticRatioAbilityValue : Value<float>
    {
        [SerializeField] private StatisticDefinition casterDefinition;
        [SerializeField, Range(0, 10)] private float ratio;

        public override float GetValue()
        {
            if (owner is not AbilityEntity ability)
                throw new Exception($"Excepting the type of {owner} to be of {nameof(AbilityEntity)}");

            return ability.Caster.Entity.GetCachedComponent<StatisticRepository>().GetOrThrow<float>(casterDefinition).GetModifiedValue(Context.Empty) * ratio;
        }

        public override bool TryGetDescription(out string description)
        {
            description = $"<color=#{casterDefinition.ColorHex}>{ratio:0.0%}{casterDefinition.TextIcon}</color>";
            return true;
        }
    }
}
