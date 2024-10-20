using Game.Character;
using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class HealProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeReference, SubclassSelector] private ProjectileStatistic heal;

        public void Execute(Collider2D collider, Target target)
        {
            if (!target.Entity.TryGetCachedComponent<CharacterEntity>(out CharacterEntity character))
                return;

            character.Heal(heal.GetValue<float>(projectile));
        }
    }
}
