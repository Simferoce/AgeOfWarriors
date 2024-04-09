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
            if (Ability.Targets.Count == 0)
                return;

            IAttackable target = Ability.Targets[0];
            target.TakeAttack(new Attack(new AttackSource(new List<IAttackSource>() { Ability.Character, this }), damagePercentage * Ability.Character.AttackPower, armorPenetration, leach));
        }

        public override AbilityEffect Clone()
        {
            DealDamageAbilityEffect dealDamageAbilityEffect = new DealDamageAbilityEffect();
            dealDamageAbilityEffect.leach = leach;
            dealDamageAbilityEffect.damagePercentage = damagePercentage;
            dealDamageAbilityEffect.armorPenetration = armorPenetration;

            return dealDamageAbilityEffect;
        }
    }
}
