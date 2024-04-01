using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IAttackSource
    {
        [SerializeField, Range(0, 3)] private float leach = 0f;
        [SerializeField, Range(0, 3)] private float damagePercentage = 1f;
        [SerializeField] private float armorPenetration = 0f;

        public override void Apply()
        {
            if (ability.Targets.Count == 0)
                return;

            IAttackable target = ability.Targets[0];
            target.TakeAttack(new Attack(new AttackSource(new List<IAttackSource>() { ability.Character, this }), damagePercentage * ability.Character.AttackPower, armorPenetration, leach));
        }
    }
}
