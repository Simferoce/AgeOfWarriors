using System;

namespace Game
{
    [Serializable]
    public abstract class TargetCriteria
    {
        public abstract bool Execute(Target owner, Target targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction);
    }
}
