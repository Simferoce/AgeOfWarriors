using System;

namespace Game
{
    [Serializable]
    public abstract class ProjectileData
    {
        public abstract ProjectileContext GetContext(Character character);
        public abstract ProjectileData Clone();
    }
}
