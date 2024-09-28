using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAttackImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private TargetCriteria criteria;
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference armorPenetration;

        private float cachedDamage;
        private float cachedArmorPenetration;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);

            damage.Initialize(projectile);
            armorPenetration.Initialize(projectile);

            cachedDamage = damage;
            cachedArmorPenetration = armorPenetration;
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && (targeteable.Entity as AgentObject).IsActive
                && projectile.Ignore != targeteable
                && criteria.Execute(projectile.Parent.GetCachedComponent<Target>(), targeteable, projectile, projectile.Faction, (targeteable.Entity as AgentObject).Faction)
                && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
            {
                AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
                Attack attack = attackFactory.Generate(
                    target: attackable,
                    damage: damage,
                    armorPenetration: armorPenetration,
                    flags: Attack.Flag.Ranged | Attack.Flag.Reflectable);

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
