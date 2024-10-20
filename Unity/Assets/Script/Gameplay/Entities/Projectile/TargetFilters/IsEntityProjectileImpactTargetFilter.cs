using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public class IsEntityProjectileImpactTargetFilter : ProjectileTargetFilter, IStandardProjectileTargetFilter
    {
        public bool Execute(Target target)
        {
            return target != null;
        }
    }
}
