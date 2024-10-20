using Game.Character;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class HealProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeReference, SubclassSelector] private ProjectileStatistic heal;

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<CharacterEntity>(out CharacterEntity character))
                return;

            character.Heal(heal.GetValue<float>(projectile));
        }
    }
}
