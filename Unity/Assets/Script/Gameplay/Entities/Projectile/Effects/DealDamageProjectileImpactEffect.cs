using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class DealDamageProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeReference, SubclassSelector] private ProjectileStatistic damage;
        [SerializeReference, SubclassSelector] private ProjectileStatistic armorPenetration;

        public float Damage => (damage?.GetValue<float>(projectile) ?? 0f) * (1 + projectile.GetCachedComponent<StatisticIndex>().SumByDefinition(StatisticIdentifiant.DamagePercentage)) * projectile.GetCachedComponent<StatisticIndex>().MultiplierByDefinition(StatisticIdentifiant.DamageMultiplier);

        public override bool Validate(ProjectileEntity projectile)
        {
            bool changed = base.Validate(projectile);
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

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: Damage,
                armorPenetration: armorPenetration?.GetValue<float>(projectile) ?? 0f,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}