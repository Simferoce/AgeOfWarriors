using Extension;
using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileHealImpact : ProjectileImpact
    {
        public override bool Impact(GameObject collision)
        {
            if (projectile.Rigidbody.velocity.y > 0)
                return false;

            ProjectileFactoryHealContext.Context healContext = (ProjectileFactoryHealContext.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileFactoryHealContext.Context);
            ProjectileFactoryImpactContext.Context targetContext = (ProjectileFactoryImpactContext.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileFactoryImpactContext.Context);

            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<IHealable>(out IHealable healable)
                && healable.IsActive
                && targetContext.Criteria.Execute(projectile.Character, healable, projectile))
            {
                healable.Heal(healContext.HealAmount);

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
