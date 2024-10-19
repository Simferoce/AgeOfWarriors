using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierBehaviour : IDisposable
    {
        public enum Result
        {
            Alive,
            Dead
        }

        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public virtual Result Update() { return Result.Alive; }
        public virtual void Refresh() { }
        public virtual void Dispose() { }
    }
}