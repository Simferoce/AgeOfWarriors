using System;

namespace Game
{
    [Serializable]
    public class IsEnemyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return targetFaction != ownerFaction;
        }
    }
}
