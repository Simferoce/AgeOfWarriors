using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAttackImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private TargetCriteria criteria;
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
                collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                && targeteable.IsActive
                && projectile.Ignore != targeteable
                && criteria.Execute(projectile.Character.GetCachedComponent<ITargeteable>(), targeteable, projectile)
                && targeteable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
            {
                attack = projectile.Character.GenerateAttack(damage.GetValueOrThrow(projectile.Ability), armorPenetration.GetValueOrDefault(projectile.Ability), 0, true, false, attackable, projectile);
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
