using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(Target owner, Target targeteable, IStatisticContext statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return (targeteable.Entity is Character) && (targeteable.Entity as AgentObject).IsActive;
        }
    }
}
