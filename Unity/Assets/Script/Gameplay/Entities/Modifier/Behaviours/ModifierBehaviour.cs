using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierBehaviour : IDisposable
    {
        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public virtual void Update() { }
        public virtual void Refresh() { }
        public virtual void Dispose() { }
    }
}