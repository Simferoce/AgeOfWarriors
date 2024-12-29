using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class HealProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeField] private StatisticReference<float> heal;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            heal.Initialize(projectile);
        }

        public void Execute(Entity entity)
        {
            if (entity is not IHealable healable)
                return;

            healable.Heal(heal.GetOrThrow().Get<float>());
        }
    }
}
