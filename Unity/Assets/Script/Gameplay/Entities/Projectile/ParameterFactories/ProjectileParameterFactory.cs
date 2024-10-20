using Game.Projectile;
using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class ProjectileParameterFactory
    {
        public abstract ProjectileParameter Create(object entity);
    }
}
