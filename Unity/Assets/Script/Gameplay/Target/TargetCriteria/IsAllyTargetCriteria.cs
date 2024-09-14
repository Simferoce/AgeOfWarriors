using System;

namespace Game
{
    [Serializable]
    public class IsAllyTargetCriteria : TargetCriteria
    {
        public override bool Execute(Target owner, Target targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return ownerFaction == targetFaction;
        }
    }
}
