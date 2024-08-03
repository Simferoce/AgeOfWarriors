using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller, Faction ownerFaction, Faction targetFaction)
        {
            return (targeteable is Character) && targeteable.IsActive;
        }
    }
}
