using System;

namespace Game
{
    [Serializable]
    public abstract class ModifierEffect
    {
        protected Modifier modifier;

        public virtual void Initialize(Modifier modifier)
        {
            this.modifier = modifier;
        }

        public abstract void Execute();
    }
}