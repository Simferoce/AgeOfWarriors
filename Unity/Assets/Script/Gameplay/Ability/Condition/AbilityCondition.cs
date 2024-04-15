using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityCondition
    {
        protected Ability ability;

        public virtual void Initialize(Ability ability)
        {
            this.ability = ability;
        }

        public abstract bool Execute();
        public virtual void OnAbilityStarted() { }
        public virtual void OnAbilityEnded() { }
        public virtual void Interrupt() { }
    }
}
