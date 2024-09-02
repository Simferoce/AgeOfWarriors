using System;

namespace Game
{
    [Serializable]
    public class IsAllyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IStatisticProviderOld statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return ownerFaction == targetFaction;
        }
    }
}
