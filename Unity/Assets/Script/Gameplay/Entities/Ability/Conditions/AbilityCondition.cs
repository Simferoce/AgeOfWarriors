using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityCondition
    {
        protected AbilityEntity ability;

        public virtual void Initialize(AbilityEntity ability)
        {
            this.ability = ability;
        }

        public abstract bool Execute();
        public virtual void OnAbilityStarted() { }
        public virtual void OnAbilityEnded() { }
        public virtual void Interrupt() { }
    }
}
