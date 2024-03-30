using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityEffect
    {
        public abstract void Apply(Character character);
        public virtual void OnAbilityEnded() { }
    }
}
