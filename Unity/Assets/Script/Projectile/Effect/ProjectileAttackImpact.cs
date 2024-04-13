using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAttackImpact : ProjectileImpact
    {
        [SerializeReference, SerializeReferenceDropdown] private TargetCriteria criteria;
        [SerializeField] private StatisticReference<float> damage;

        private Attack attack;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);

            attack = new Attack(new AttackSource(projectile.Character, projectile), damage.GetValueOrThrow(projectile), 0, 0);
        }

        public override bool Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                && targeteable.IsActive
                && criteria.Execute(projectile.Character, targeteable, projectile)
                && targeteable is IAttackable attackable)
            {
                attackable.TakeAttack(attack);

                return true;
            }
            else if (collision.gameObject.CompareTag(GameTag.GROUND))
            {
                return true;
            }

            return false;
        }
    }
}
