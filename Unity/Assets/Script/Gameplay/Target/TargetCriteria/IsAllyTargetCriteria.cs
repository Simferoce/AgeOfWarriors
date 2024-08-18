using System;

namespace Game
{
    [Serializable]
    public class IsAllyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IContext context, Faction ownerFaction, Faction targetFaction)
        {
            return ownerFaction == targetFaction;
        }
    }
}
