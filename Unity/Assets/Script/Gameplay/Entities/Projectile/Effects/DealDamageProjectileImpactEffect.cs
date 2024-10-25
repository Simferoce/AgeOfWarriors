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

        public void Execute(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
                return;

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: damage?.Resolve(projectile)?.GetValue<float>(null) ?? 0f,
                armorPenetration: armorPenetration?.Resolve(projectile)?.GetValue<float>(null) ?? 0f,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}