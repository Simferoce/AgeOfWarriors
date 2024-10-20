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

        public float Damage => (damage?.GetValue<float>(Ability) ?? 0f) * (1 + Ability.GetCachedComponent<StatisticIndex>().SumByDefinition(StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.DamagePercentage)));
        public float ArmorPenetration => armorPenetration?.GetValue<float>(Ability) ?? 0f;
        public float Leach => leach?.GetValue<float>(Ability) ?? 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
        }

        public override bool Validate()
        {
            bool changed = base.Validate();
            if (leach != null && leach.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Leach))
            {
                leach.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Leach);
                changed = true;
            }
            if (damage != null && damage.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Damage))
            {
                damage.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Damage);
                changed = true;
            }
            if (armorPenetration != null && armorPenetration.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.ArmorPenetration))
            {
                armorPenetration.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.ArmorPenetration);
                changed = true;
            }

            return changed;
        }

        public override void Apply()
        {
            if (Ability.Targets.Count == 0)
                return;

            Attackable target = Ability.Targets[0].Entity.GetCachedComponent<Attackable>();

            AttackData attack = Ability.GetCachedComponent<AttackFactory>().Generate(
                target: target,
                damage: Damage,
                armorPenetration: ArmorPenetration,
                leach: Leach);

            target.TakeAttack(attack);
        }
    }
}
