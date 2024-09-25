using System;

namespace Game
{
    [Serializable]
    public abstract class ModifierBehaviour : IDisposable
    {
        public abstract void Initialize();
        public abstract bool Update();
        public abstract void Refresh();
        public virtual void Dispose() { }
    }
}