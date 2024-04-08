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
        private float startedAt;
        private float duration;
        private TargetCriteria criteria;
        private List<IAttackable> targets = new List<IAttackable>();
        private Attack attack;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            ProjectileDurationData.Context durationContext = (ProjectileDurationData.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileDurationData.Context);
            ProjectileImpactData.Context targetContext = (ProjectileImpactData.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileImpactData.Context);
            ProjectileAttackData.Context attackContext = (ProjectileAttackData.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileAttackData.Context);

            attack = attackContext.Attack;
            attack.AttackSource.Sources.Add(projectile);
            duration = durationContext.Duration;
            criteria = targetContext.Criteria;
        }

        public override bool Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<IAttackable>(out IAttackable attackable)
                && criteria.Execute(projectile.Character, attackable, null))
            {
                targets.Add(attackable);
            }

            return false;
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

        public override bool Update()
        {
            if (projectile.StateValue == Projectile.State.Dead)
                return false;

            if (Time.time - startedAt > duration)
            {
                foreach (IAttackable target in targets)
                {
                    target.TakeAttack(attack);
                }

                return true;
            }

            return false;
        }
    }
}
