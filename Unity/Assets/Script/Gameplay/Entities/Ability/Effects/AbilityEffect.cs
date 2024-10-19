using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityEffect
    {
        public AbilityEntity Ability { get; protected set; }

        public virtual void Initialize(AbilityEntity ability)
        {
            this.Ability = ability;
        }

        public virtual bool Validate() { return false; }
        public abstract void Apply();
        public virtual bool CanBeApplied() { return true; }
        public virtual void OnAbilityEnded() { }
        public virtual void OnAbilityStarted() { }
        public virtual void Interrupt() { }
    }
}
