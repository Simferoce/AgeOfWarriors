using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierTargetFilter
    {
        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public abstract bool Execute(Entity target);
    }
}
