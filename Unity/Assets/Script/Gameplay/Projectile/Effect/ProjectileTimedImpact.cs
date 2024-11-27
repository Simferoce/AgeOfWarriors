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
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> duration;
        [SerializeField] private float delay;

        private float startedAt;
        private List<Attackable> targetsHit = new List<Attackable>();
        private float currentDuration;
        private List<ITargeteable> targeteablesInEffect = new List<ITargeteable>();

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            currentDuration = duration.GetValueOrThrow(projectile);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX)
                && collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
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
                && collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                && targeteablesInEffect.Contains(targeteable))
            {
                targeteablesInEffect.Remove(targeteable);
            }
        }

        public override ImpactReport Update()
        {
            if (projectile.StateValue == Projectile.State.Dead)
                return new ImpactReport(ImpactStatus.NotImpacted);

            if (Time.time - startedAt > currentDuration)
            {
                projectile.Kill(null);
            }

            List<ITargeteable> targeteablesHitThisFrame = new List<ITargeteable>();
            foreach (ITargeteable targeteable in targeteablesInEffect)
            {
                if (Time.time - startedAt > delay
                    && criteria.Execute(projectile.AgentObject.GetCachedComponent<ITargeteable>(), targeteable, projectile, projectile.Faction, targeteable.Faction)
                    && targeteable.TryGetCachedComponent<Attackable>(out Attackable attackable)
                    && !targetsHit.Contains(attackable))
                {
                    Attack attack = projectile.GetCachedComponent<AttackFactory>().Generate(damage.GetValueOrThrow(projectile), 0, 0, true, false, true, attackable);
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
