using Extension;
using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileHealImpact : ProjectileImpact
    {
        public override bool Impact(GameObject collision, Projectile projectile)
        {
            if (projectile.Rigidbody.velocity.y > 0)
                return false;

            ProjectileHealData.Context healContext = (ProjectileHealData.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileHealData.Context);
            ProjectileImpactData.Context targetContext = (ProjectileImpactData.Context)projectile.Contexts.FirstOrDefault(x => x is ProjectileImpactData.Context);

            if (collision.CompareTag(GameTag.HIT_BOX) &&
                collision.gameObject.TryGetComponentInParent<IHealable>(out IHealable healable)
                && targetContext.Criteria.Execute(projectile.Character, healable))
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
