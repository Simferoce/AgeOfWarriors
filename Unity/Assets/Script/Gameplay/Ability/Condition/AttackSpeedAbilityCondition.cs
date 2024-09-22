using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AttackSpeedAbilityCondition : AbilityCondition
    {
        [SerializeField] private StatisticReference attackSpeed;

        public override void Initialize(Ability ability)
        {
            base.Initialize(ability);
            attackSpeed.Initialize(ability);
        }

        public override bool Execute()
        {
            return Time.time - ability.Caster.LastAbilityUsed > 1 / attackSpeed;
        }
    }
}
