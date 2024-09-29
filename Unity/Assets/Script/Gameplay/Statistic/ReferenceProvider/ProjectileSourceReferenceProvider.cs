using System;

namespace Game
{
    [Serializable]
    public class ProjectileSourceReferenceProvider : ReferenceProvider
    {
        public override object Resolve(object context)
        {
            if (!(context is Projectile projectile))
                throw new ArgumentException();

            return projectile.Parent;
        }
    }
}
