using System;
using System.Collections.Generic;
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
            target.TakeAttack(new Attack(new AttackSource(new List<IAttackSource>() { Ability.Character, this }), damage.GetValueOrDefault(Ability), armorPenetration.GetValueOrDefault(Ability), leach.GetValueOrDefault(Ability)));
        }
    }
}
