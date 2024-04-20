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
        [SerializeField] private StatisticReference<float> armorPenetration;

        private Attack attack;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);
        }

        public override bool Impact(GameObject collision)
        {
            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable)
                && targeteable.IsActive
                && criteria.Execute(projectile.Character.GetCachedComponent<ITargeteable>(), targeteable, projectile)
                && targeteable.TryGetCachedComponent<IAttackable>(out IAttackable attackable))
            {
                attack = projectile.Character.GenerateAttack(damage.GetValueOrThrow(projectile), armorPenetration.GetValueOrDefault(projectile), 0, true, attackable, projectile);
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
