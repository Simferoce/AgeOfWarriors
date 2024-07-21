using System;

namespace Game
{
    [Serializable]
    public abstract class PoolEffect
    {
        public virtual void Initialize(Pool pool) { }
        public virtual void Apply(Pool pool, ITargeteable targeteable) { }
    }
}
