using System;

namespace Game
{
    [Serializable]
    public abstract class AbilityTargetCriteria
    {
        public abstract bool Execute(Ability source, Entity targetEntity);
    }
}
