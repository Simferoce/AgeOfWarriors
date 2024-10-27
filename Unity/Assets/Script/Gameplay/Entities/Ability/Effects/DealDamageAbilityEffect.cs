using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        [SerializeReference, SubclassSelector] private Statistic leach;
        [SerializeReference, SubclassSelector] private Statistic damage;
        [SerializeReference, SubclassSelector] private Statistic armorPenetration;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            leach?.Initialize(ability);
            damage?.Initialize(ability);
            armorPenetration?.Initialize(ability);
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            AttackData attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: damage?.GetValue<float>(null) ?? 0f,
                armorPenetration: leach?.GetValue<float>(null) ?? 0f,
                leach: armorPenetration?.GetValue<float>(null) ?? 0f);

            target.TakeAttack(attack);
        }
    }
}
