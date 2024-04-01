using System;

namespace Game
{
    [Serializable]
    public abstract class ProjectileContext
    {
        protected Projectile projectile;

        public virtual void Initialize(Projectile projectile)
        {

        }
    }
}
