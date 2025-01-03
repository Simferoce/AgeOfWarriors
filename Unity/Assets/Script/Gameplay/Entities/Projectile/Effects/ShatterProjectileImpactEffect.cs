using Game.Components;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class ShatterProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> subsequenceDamage;
        [SerializeField] private StatisticReference<float> range;
        [SerializeReference, SubclassSelector] private IStandardProjectileTargetFilter filter;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            damage?.Initialize(projectile);
            subsequenceDamage?.Initialize(projectile);
            range?.Initialize(projectile);
            filter?.Initialize(projectile);
        }

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            float damageValue = (damage?.Get()?.Get<float>() ?? 0f) * (projectile.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.MultiplierDamage, out Statistic statistic) ? statistic.Get<float>() : 1f);
            float subsequentDamageValue = (subsequenceDamage?.Get()?.Get<float>() ?? 0f) * (projectile.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.MultiplierDamage, out Statistic statisticSubsequent) ? statisticSubsequent.Get<float>() : 1f);

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: damageValue,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);

            ProjectileParameter<float> projectileParameter = projectile.Parameters.OfType<ProjectileParameter<float>>().FirstOrDefault(x => x.Name == "direction");
            if (projectileParameter == null)
            {
                Debug.LogError($"Could not find a parameter with the name \"direction\".");
                return;
            }

            foreach (Target target in Target.All)
            {
                if (!target.Entity.TryGetCachedComponent<Attackable>(out Attackable subsequentAttackable))
                    continue;

                if (attackable == subsequentAttackable)
                    continue;

                if (Mathf.Abs(target.CenterPosition.x - entity.transform.position.x) > range.Get().Get<float>())
                    continue;

                if (Mathf.Sign(target.CenterPosition.x - entity.transform.position.x) != projectileParameter.GetValue())
                    continue;

                if (filter != null && !filter.Execute(target))
                    continue;

                AttackData subsequentAttack = attackFactory.Generate(
                    target: subsequentAttackable,
                    damage: subsequentDamageValue,
                    flags: AttackData.Flag.Ranged);

                subsequentAttackable.TakeAttack(subsequentAttack);
            }
        }
    }
}