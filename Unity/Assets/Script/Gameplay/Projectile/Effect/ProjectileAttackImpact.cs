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

        private Attack attack;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && (targeteable.Entity as AgentObject).IsActive
                && projectile.Ignore != targeteable
                && criteria.Execute(projectile.AgentObject.GetCachedComponent<Target>(), targeteable, projectile, projectile.Faction, (targeteable.Entity as AgentObject).Faction)
                && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
            {
                attack = AttackUtility.Generate(projectile.AgentObject as IAttackSource, damage.GetValueOrThrow<float>(projectile), armorPenetration.GetValueOrDefault<float>(projectile), 0, true, false, true, attackable, projectile);
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
