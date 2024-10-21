using System;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileEffect
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public virtual bool Validate(ProjectileEntity projectile) { return false; }
    }
}
