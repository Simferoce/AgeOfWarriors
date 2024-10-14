using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DealDamageAbilityEffect : AbilityEffect
    {
        [SerializeReference, SubclassSelector] private Statistic leach;
        [SerializeReference, SubclassSelector] private Statistic damage;
        [SerializeReference, SubclassSelector] private Statistic armorPenetration;

        public float Damage => (damage?.GetValue<float>(Ability) ?? 0f) * (1 + Ability.GetCachedComponent<StatisticIndex>().SumByDefinition(StatisticRepository.DamagePercentage));
        public float ArmorPenetration => armorPenetration?.GetValue<float>(Ability) ?? 0f;
        public float Leach => leach?.GetValue<float>(Ability) ?? 0f;

        public override void Initialize(Ability ability)
        {
            base.Initialize(ability);
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            Attack attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: Damage,
                armorPenetration: ArmorPenetration,
                leach: Leach,
                flags: Attack.Flag.Reflectable);

            target.TakeAttack(attack);
        }
    }
}
