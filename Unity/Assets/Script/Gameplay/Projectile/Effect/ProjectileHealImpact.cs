using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileHealImpact : ProjectileImpact
    {
        [SerializeField] private ProjectileStatistic heal;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (projectile.Rigidbody.linearVelocity.y > 0)
                return new ImpactReport(ImpactStatus.NotImpacted);

            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && (targeteable.Entity as AgentObject).IsActive
                && projectile.Faction == (targeteable.Entity as AgentObject).Faction
                && targeteable.Entity is Character character)
            {
                character.Heal(heal.GetValue<float>(projectile));

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
