using Game.Components;
using System;

namespace Game.Pool
{
    [Serializable]
    public abstract class PoolEffect
    {
        public virtual void Initialize(PoolEntity pool) { }
        public virtual void Apply(PoolEntity pool, Target targeteable) { }
        public virtual void Dispose() { }
    }
}
