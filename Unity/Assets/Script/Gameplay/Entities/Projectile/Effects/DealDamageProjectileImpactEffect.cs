using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class DealDamageProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> armorPenetration;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            damage?.Initialize(projectile);
            armorPenetration?.Initialize(projectile);
        }

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;


            float damageValue = (damage?.Get()?.Get<float>() ?? 0f) * (projectile.StatisticRepository.TryGet(StatisticDefinitionRegistry.Instance.MultiplierDamage, out Statistic statistic) ? statistic.Get<float>() : 1f);

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: damageValue,
                armorPenetration: armorPenetration?.Get()?.Get<float>() ?? 0f,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}