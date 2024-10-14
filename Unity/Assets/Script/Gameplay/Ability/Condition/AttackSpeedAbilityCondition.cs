using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AttackSpeedAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            return Time.time - ability.Caster.LastAbilityUsed > 1f / ability.Caster.Entity.GetCachedComponent<StatisticIndex>().SelfByDefinition<float>(StatisticRepository.AttackSpeed);
        }
    }
}
