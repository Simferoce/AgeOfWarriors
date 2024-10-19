using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class CasterStatisticRatioAbilityStatistic : AbilityStatistic
    {
        [SerializeField] private StatisticDefinition casterDefinition;
        [SerializeField, Range(0, 10)] private float ratio;

        public override T GetValue<T>(object context)
        {
            if (context is not AbilityEntity ability)
                throw new Exception($"Excepting the type of {context} to be of {nameof(AbilityEntity)}");

            return StatisticConverter.ConvertGeneric<T, float>(ability.Caster.Entity.GetCachedComponent<StatisticIndex>().SelfByDefinition<float>(casterDefinition) * ratio);
        }
    }
}
