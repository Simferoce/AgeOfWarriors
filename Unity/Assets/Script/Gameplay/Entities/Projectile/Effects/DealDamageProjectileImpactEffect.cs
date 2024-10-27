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

        private float cachedDamage;
        private float cachedArmorPenetration;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            cachedDamage = damage?.Resolve(projectile)?.GetValue<float>(null) ?? 0f;
            cachedArmorPenetration = armorPenetration?.Resolve(projectile)?.GetValue<float>(null) ?? 0f;
        }

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: cachedDamage,
                armorPenetration: cachedArmorPenetration,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}