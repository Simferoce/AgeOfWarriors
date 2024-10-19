using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierTrigger : IDisposable
    {
        public delegate void OnTriggerDelegate(ModifierTrigger trigger);
        public event OnTriggerDelegate OnTrigger;

        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        protected void Trigger()
        {
            OnTrigger?.Invoke(this);
        }

        public virtual void Dispose()
        {

        }
    }
}
