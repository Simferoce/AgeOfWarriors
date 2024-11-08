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

        public float Damage => damage?.Get().GetModifiedValue<float>() ?? 0f;
        public float ArmorPenetration => armorPenetration?.Get().GetModifiedValue<float>() ?? 0f;

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

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: Damage,
                armorPenetration: ArmorPenetration,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}