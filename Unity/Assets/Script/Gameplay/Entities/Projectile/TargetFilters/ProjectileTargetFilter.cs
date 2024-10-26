using System;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileTargetFilter
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }
    }
}
