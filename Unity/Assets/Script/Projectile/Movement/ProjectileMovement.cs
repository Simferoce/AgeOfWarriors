using System;

namespace Game
{
    [Serializable]
    public abstract class ProjectileMovement
    {
        protected Projectile projectile;

        public virtual void Initialize(Projectile projectile)
        {
            this.projectile = projectile;
        }

        public abstract void Update();
    }
}
