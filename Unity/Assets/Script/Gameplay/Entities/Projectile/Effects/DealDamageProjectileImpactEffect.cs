using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class DealDamageProjectileImpactEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeReference, SubclassSelector] private Value damage;
        [SerializeReference, SubclassSelector] private Value armorPenetration;

        public float Damage => damage?.GetValue<float>() ?? 0f;
        public float ArmorPenetration => armorPenetration?.GetValue<float>() ?? 0f;

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