using Game.Projectile;
using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityProjectileParameterFactory
    {
        public abstract ProjectileParameter Create(AbilityEntity ability);
    }
}
