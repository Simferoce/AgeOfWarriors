using System;

namespace Game
{
    [Serializable]
    public class IsEnemyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller, Faction ownerFaction, Faction targetFaction)
        {
            return targetFaction != ownerFaction;
        }
    }
}
