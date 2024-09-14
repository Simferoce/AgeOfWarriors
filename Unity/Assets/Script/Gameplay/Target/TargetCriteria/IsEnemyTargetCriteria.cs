using System;

namespace Game
{
    [Serializable]
    public class IsEnemyTargetCriteria : TargetCriteria
    {
        public override bool Execute(Target owner, Target targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return targetFaction != ownerFaction;
        }
    }
}
