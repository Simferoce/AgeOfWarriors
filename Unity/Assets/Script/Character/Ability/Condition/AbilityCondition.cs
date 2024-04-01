using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityCondition
    {
        protected Character character;

        public virtual void Initialize(Character character)
        {
            this.character = character;
        }

        public abstract bool Execute();
        public virtual void OnAbilityStarted() { }
        public virtual void OnAbilityEnded() { }
        public virtual void Interrupt() { }
    }
}
