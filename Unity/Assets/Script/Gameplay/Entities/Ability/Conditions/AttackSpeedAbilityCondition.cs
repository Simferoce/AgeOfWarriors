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
            float attackSpeed = ability.Caster.Entity.GetStatisticOrThrow<float>(StatisticDefinitionRegistry.Instance.AttackSpeed);
            return Time.time - ability.Caster.LastAbilityUsed > 1f / attackSpeed;
        }
    }
}
