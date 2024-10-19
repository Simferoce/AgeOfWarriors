using System;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileMovement
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public abstract void Update();
    }
}
