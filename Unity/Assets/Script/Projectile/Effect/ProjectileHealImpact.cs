using Extension;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileHealImpact : ProjectileImpact
    {
        [SerializeReference, SerializeReferenceDropdown] private TargetCriteria criteria;
        [SerializeField] private StatisticReference<float> heal;

        private float currentHeal;

        public override void Initialize(Projectile projectile)
        {
            base.Initialize(projectile);

            currentHeal = heal.GetValueOrThrow(projectile);
        }

        public override bool Impact(GameObject collision)
        {
            if (projectile.Rigidbody.velocity.y > 0)
                return false;

            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<IHealable>(out IHealable healable)
                && healable.IsActive
                && criteria.Execute(projectile.Character, healable, projectile))
            {
                healable.Heal(currentHeal);

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
