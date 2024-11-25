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

            AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
            Context context = new AttackContext(attackable);

            AttackData attack = attackFactory.Generate(
                target: attackable,
                damage: damage?.Get()?.GetModifiedValue<float>(context) ?? 0f,
                armorPenetration: armorPenetration?.Get()?.GetModifiedValue<float>(context) ?? 0f,
                flags: AttackData.Flag.Ranged);

            attackable.TakeAttack(attack);
        }
    }
}