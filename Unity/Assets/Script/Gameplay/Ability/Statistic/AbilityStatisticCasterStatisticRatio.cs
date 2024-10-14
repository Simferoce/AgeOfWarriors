using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityStatisticCasterStatisticRatio : AbilityStatistic
    {
        [SerializeField] private StatisticDefinition casterDefinition;
        [SerializeField, Range(0, 10)] private float ratio;

        public override StatisticDefinition GetDefinition(object context)
        {
            throw new NotImplementedException();
        }

        public override T GetValue<T>(object context)
        {
            if (context is not Ability ability)
                throw new Exception($"Excepting the type of {context} to be of {nameof(Ability)}");

            return StatisticUtility.ConvertGeneric<T, float>(ability.Caster.Entity.GetCachedComponent<StatisticIndex>().SelfByDefinition<float>(casterDefinition) * ratio);
        }
    }
}
