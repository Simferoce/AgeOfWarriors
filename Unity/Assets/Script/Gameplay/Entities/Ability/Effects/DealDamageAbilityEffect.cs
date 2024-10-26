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
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            AttackData attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: damage?.Resolve(Ability)?.GetValue<float>(null) ?? 0f,
                armorPenetration: leach?.Resolve(Ability)?.GetValue<float>(null) ?? 0f,
                leach: armorPenetration?.Resolve(Ability)?.GetValue<float>(null) ?? 0f);

            target.TakeAttack(attack);
        }
    }
}
