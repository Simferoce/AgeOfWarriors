using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityEffect
    {
        protected CharacterAbility ability;

        public virtual void Initialize(CharacterAbility ability)
        {
            this.ability = ability;
        }

        public abstract void Apply();
        public virtual bool CanBeApplied() { return true; }
        public virtual void OnAbilityEnded() { }
        public virtual void OnAbilityStarted() { }
        public virtual void Interrupt() { }
    }
}
