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

        public void Execute(Collider2D collider, Target target)
        {
            if (!target.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: damage?.GetValue<float>(projectile) ?? 0f,
                armorPenetration: armorPenetration?.GetValue<float>(projectile) ?? 0f,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}