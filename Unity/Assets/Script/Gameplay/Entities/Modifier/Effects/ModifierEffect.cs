using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierEffect
    {
        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public abstract void Execute();
    }
}