using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AttackSpeedAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            return Time.time - ability.Character.LastAbilityUsed > ability.Character.AttackSpeed;
        }
    }
}
