using System;

namespace Game
{
    [Serializable]
    public abstract class ProjectileFactoryContext
    {
        public abstract ProjectileContext GetContext(Character character);
    }
}
