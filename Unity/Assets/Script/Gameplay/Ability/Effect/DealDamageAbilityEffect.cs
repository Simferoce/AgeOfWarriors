using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IAttackSource
    {
        [SerializeField] private StatisticReference<float> leach;
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> armorPenetration;

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            IAttackable target = Ability.Targets[0].GetCachedComponent<IAttackable>();

            Attack attack = Ability.Character.GenerateAttack(damage.GetValueOrDefault(new Context() { { "ability", Ability } }), armorPenetration.GetValueOrDefault(new Context() { { "ability", Ability } }), leach.GetValueOrDefault(new Context() { { "ability", Ability } }), false, false, true, target, this);
            target.TakeAttack(attack);
        }
    }
}
