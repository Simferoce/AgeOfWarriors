using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect, IAttackSource
    {
        [SerializeField] private StatisticReference leach;
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference armorPenetration;

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            Attack attack = AttackUtility.Generate(Ability.Caster.Entity as IAttackSource, damage, armorPenetration, leach, false, false, true, target, this);
            target.TakeAttack(attack);
        }
    }
}
