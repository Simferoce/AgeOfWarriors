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

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<CharacterEntity>(out CharacterEntity character))
                return;

            //character.Heal(heal.GetValue<float>(projectile));
        }
    }
}
