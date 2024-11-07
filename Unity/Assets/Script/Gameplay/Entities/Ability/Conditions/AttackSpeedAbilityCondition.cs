using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class AttackSpeedAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            float attackSpeed = ability.Caster.Entity.GetCachedComponent<StatisticRepository>().GetOrThrow(StatisticDefinitionRegistry.Instance.AttackSpeed).GetModifiedValue();
            return Time.time - ability.Caster.LastAbilityUsed > 1f / attackSpeed;
        }
    }
}
