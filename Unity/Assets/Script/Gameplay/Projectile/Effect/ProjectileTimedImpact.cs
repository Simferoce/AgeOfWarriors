using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileTimedImpact : ProjectileImpact
    {
        [SerializeReference, SerializeReferenceDropdown] private TargetCriteria criteria;
        [SerializeField] private StatisticReference<float> damage;
        [SerializeField] private StatisticReference<float> duration;

        private float startedAt;
        private List<IAttackable> targets = new List<IAttackable>();
        private Attack attack;
        private float currentDuration;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            currentDuration = duration.GetValueOrThrow(projectile.Ability);
        }

        public override ImpactReport Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                && criteria.Execute(projectile.Character.GetCachedComponent<ITargeteable>(), targeteable, projectile)
                && targeteable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
            {
                attack = projectile.Character.GenerateAttack(damage.GetValueOrThrow(projectile), 0, 0, true, false, attackable, projectile);
                attack.AttackSource.Sources.Add(projectile);
                targets.Add(attackable);
            }

            return new ImpactReport(ImpactStatus.NotImpacted);
        }

        public override void LeaveZone(GameObject collision)
        {
            base.LeaveZone(collision);

            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<IAttackable>(out IAttackable attackable))
            {
                targets.Remove(attackable);
            }
        }

        public override ImpactReport Update()
        {
            if (projectile.StateValue == Projectile.State.Dead)
                return new ImpactReport(ImpactStatus.NotImpacted);

            if (Time.time - startedAt > currentDuration)
            {
                foreach (IAttackable target in targets)
                {
                    target.TakeAttack(attack);
                }

                return new ImpactReport(ImpactStatus.Impacted, targets.Select(x => x.GetCachedComponent<ITargeteable>()).ToList());
            }

            return new ImpactReport(ImpactStatus.NotImpacted);
        }
    }
}
