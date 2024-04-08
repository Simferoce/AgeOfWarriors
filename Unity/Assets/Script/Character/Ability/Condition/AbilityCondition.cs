using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityCondition
    {
        protected CharacterAbility ability;

        public virtual void Initialize(CharacterAbility ability)
        {
            this.ability = ability;
        }

        public abstract bool Execute();
        public virtual void OnAbilityStarted() { }
        public virtual void OnAbilityEnded() { }
        public virtual void Interrupt() { }
        public abstract AbilityCondition Clone();
    }
}
