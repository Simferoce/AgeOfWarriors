using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        [SerializeField] private StatisticReference leach;
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference armorPenetration;

        public override void Initialize(Ability ability)
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

            Attack attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: damage.GetValueOrDefault<float>(),
                armorPenetration: armorPenetration.GetValueOrDefault<float>(),
                leach: leach.GetValueOrDefault<float>(),
                flags: Attack.Flag.Reflectable);

            target.TakeAttack(attack);
        }
    }
}
