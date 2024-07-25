using System;

namespace Game
{
    [Serializable]
    public abstract class TargetCriteria
    {
        public abstract bool Execute(ITargeteable owner, ITargeteable targeteable, object caller, Faction ownerFaction, Faction targetFaction);
    }
}
