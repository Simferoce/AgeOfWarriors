using Game.Character;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class HealProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeField] private StatisticReference<float> heal;

        private float cachedHeal;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            cachedHeal = heal.Resolve(projectile).GetValue<float>(null);
        }

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<CharacterEntity>(out CharacterEntity character))
                return;

            character.Heal(cachedHeal);
        }
    }
}
