using Extension;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileTimedImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private TargetCriteria criteria;
        [SerializeField] private StatisticReference damage;
        [SerializeField] private StatisticReference duration;
        [SerializeField] private float delay;

        private float startedAt;
        private List<Attackable> targetsHit = new List<Attackable>();
        private List<Target> targeteablesInEffect = new List<Target>();
        private float cachedDuration;
        private float cachedDamage;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            damage.Initialize(projectile);
            duration.Initialize(projectile);
            cachedDuration = duration;
            cachedDamage = damage;
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
            if (projectile.StateValue == Projectile.State.Dead)
                return new ImpactReport(ImpactStatus.NotImpacted);

            if (Time.time - startedAt > cachedDuration)
            {
                projectile.Kill(null);
            }

            List<Target> targeteablesHitThisFrame = new List<Target>();
            foreach (Target targeteable in targeteablesInEffect)
            {
                if (Time.time - startedAt > delay
                    && criteria.Execute(projectile, targeteable.Entity)
                    && targeteable.Entity.TryGetCachedComponent<Attackable>(out Attackable attackable)
                    && !targetsHit.Contains(attackable))
                {
                    AttackFactory attackFactory = projectile.GetCachedComponent<AttackFactory>();
                    Attack attack = attackFactory.Generate(
                        target: attackable,
                        damage: cachedDamage,
                        flags: Attack.Flag.Ranged | Attack.Flag.Reflectable
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
