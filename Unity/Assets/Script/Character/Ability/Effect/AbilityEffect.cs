using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityEffect
    {
        public CharacterAbility Ability { get; protected set; }

        public virtual void Initialize(CharacterAbility ability)
        {
            this.Ability = ability;
        }

        public abstract void Apply();
        public virtual bool CanBeApplied() { return true; }
        public virtual void OnAbilityEnded() { }
        public virtual void OnAbilityStarted() { }
        public virtual void Interrupt() { }
    }
}
