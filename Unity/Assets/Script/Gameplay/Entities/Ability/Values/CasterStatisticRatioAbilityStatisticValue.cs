using Game.Statistics;
using System;
using System.Linq;
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

            return ability.Caster.Entity.GetCachedComponent<StatisticRegistry>().Statistics.FirstOrDefault(x => x.Definition == casterDefinition).GetValue<float>(context) * ratio;
        }
    }
}
