using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IStatisticProvider statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            return (targeteable is Character) && targeteable.IsActive;
        }
    }
}
