using System;

namespace Game
{
    [Serializable]
    public abstract class ModifierTrigger : IDisposable
    {
        public delegate void OnTriggerDelegate(ModifierTrigger trigger);
        public event OnTriggerDelegate OnTrigger;

        protected Modifier modifier;

        public virtual void Initialize(Modifier modifier)
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
