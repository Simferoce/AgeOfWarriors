using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public class IsGoingDownProjectileImpactTargetFilter : ProjectileTargetFilter, IStandardProjectileTargetFilter
    {
        public bool Execute(Target target)
        {
            return projectile.Rigidbody.linearVelocity.y < 0;
        }
    }
}