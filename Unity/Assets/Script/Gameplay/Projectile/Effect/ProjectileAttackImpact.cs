using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAttackImpact : ProjectileImpact
    {
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> armorPenetration;

        private Attack attack;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && targeteable.Entity.IsActive
                && projectile.Ignore != targeteable
                && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable))
            {
                attack = projectile.GetCachedComponent<AttackFactory>().Generate(damage.GetValueOrThrow(projectile), armorPenetration.GetValueOrDefault(projectile), 0, true, false, true, attackable);
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
