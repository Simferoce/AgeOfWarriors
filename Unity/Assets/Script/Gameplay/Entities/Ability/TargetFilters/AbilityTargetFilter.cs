using System;

namespace Game.Ability
{
    [Serializable]
    public abstract class AbilityTargetFilter
    {
        public virtual bool Validate() { return false; }
        public abstract bool Execute(AbilityEntity source, Entity targetEntity);
    }
}
