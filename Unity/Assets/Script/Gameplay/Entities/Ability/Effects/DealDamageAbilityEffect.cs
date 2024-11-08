using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference<float> leach;
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> armorPenetration;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            leach.Initialize(ability);
            damage.Initialize(ability);
            armorPenetration.Initialize(ability);
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            AttackData attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: damage.Get()?.GetModifiedValue() ?? 0f,
                armorPenetration: armorPenetration.Get()?.GetModifiedValue() ?? 0f,
                leach: leach.Get()?.GetModifiedValue() ?? 0f);

            target.TakeAttack(attack);
        }
    }
}
