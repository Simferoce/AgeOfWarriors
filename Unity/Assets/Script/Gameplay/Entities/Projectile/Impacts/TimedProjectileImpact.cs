using Game.Agent;
using Game.Components;
using Game.Extensions;
using Game.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class TimedProjectileImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private ProjectileStatistic damage;
        [SerializeReference, SubclassSelector] private ProjectileStatistic duration;
        [SerializeReference, SubclassSelector] private ProjectileStatistic delay;

        private float startedAt;
        private List<Attackable> targetsHit = new List<Attackable>();
        private List<Target> targeteablesInEffect = new List<Target>();

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX)
                && collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && !targeteablesInEffect.Contains(targeteable))
            {
                targeteablesInEffect.Add(targeteable);
            }

            return new ImpactReport(ImpactStatus.NotImpacted);
        }

        public override void LeaveZone(GameObject collision)
        {
            base.LeaveZone(collision);

            if (collision.CompareTag(GameTag.HIT_BOX)
                && collision.gameObject.TryGetComponentInParent<Target>(out Target targeteable)
                && targeteablesInEffect.Contains(targeteable))
            {
                targeteablesInEffect.Remove(targeteable);
            }
        }

        public override ImpactReport Update()
        {
            if (projectile.StateValue == ProjectileEntity.State.Dead)
                return new ImpactReport(ImpactStatus.NotImpacted);

            if (Time.time - startedAt > duration.GetValue<float>(projectile))
            {
                projectile.Kill(null);
            }

            List<Target> targeteablesHitThisFrame = new List<Target>();
            foreach (Target targeteable in targeteablesInEffect)
            {
                if (Time.time - startedAt > delay.GetValue<float>(projectile)
                    && projectile.Faction != (targeteable.Entity as AgentObject).Faction
                    && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable)
                    && !targetsHit.Contains(attackable))
                {
                    AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
                    AttackData attack = attackFactory.Generate(
                        target: attackable,
                        damage: damage.GetValue<float>(projectile),
                        flags: AttackData.Flag.Ranged | AttackData.Flag.Reflectable
                        );

                    attackable.TakeAttack(attack);
                    targetsHit.Add(attackable);

                    targeteablesHitThisFrame.Add(targeteable);
                }
            }

            if (targeteablesHitThisFrame.Count > 0)
                return new ImpactReport(ImpactStatus.Impacted, targeteablesHitThisFrame);

            return new ImpactReport(ImpactStatus.NotImpacted);
        }
    }
}
