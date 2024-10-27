using Game.Projectile;
using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class ProjectileParameterFactory
    {
        public virtual void Initialize(Entity entity) { }
        public abstract ProjectileParameter Create(object entity);
    }
}
