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
            if (owner.Owner is not AbilityEntity ability)
                throw new Exception($"Excepting the type of {owner} to be of {nameof(AbilityEntity)}");

            return ability.Caster.Entity.GetCachedComponent<StatisticRepository>().GetOrThrow<float>(casterDefinition).Get<float>() * ratio;
        }
    }
}
