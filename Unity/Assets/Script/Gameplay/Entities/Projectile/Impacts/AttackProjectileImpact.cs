using Game.Agent;
using Game.Components;
using Game.Extensions;
using Game.Utilities;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class AttackProjectileImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private ProjectileStatistic damage;
        [SerializeReference, SubclassSelector] private ProjectileStatistic armorPenetration;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && (targeteable.Entity as AgentObject).IsActive
                && projectile.Ignore != targeteable
                && projectile.Faction != (targeteable.Entity as AgentObject).Faction
                && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
            {
                AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
                AttackData attack = attackFactory.Generate(
                    target: attackable,
                    damage: damage?.GetValue<float>(projectile) ?? 0f,
                    armorPenetration: armorPenetration?.GetValue<float>(projectile) ?? 0f,
                    flags: AttackData.Flag.Ranged | AttackData.Flag.Reflectable);

                attackable.TakeAttack(attack);

                projectile.Kill(collision.gameObject);
                return new ImpactReport(ImpactStatus.Impacted, targeteable);
            }
            else if (collision.gameObject.CompareTag(GameTag.GROUND))
            {
                projectile.Kill(collision.gameObject);
                return new ImpactReport(ImpactStatus.Impacted);
            }

            return new ImpactReport(ImpactStatus.NotImpacted);
        }
    }
}
