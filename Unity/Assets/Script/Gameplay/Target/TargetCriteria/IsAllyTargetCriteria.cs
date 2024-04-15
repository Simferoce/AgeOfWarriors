using System;

namespace Game
{
    [Serializable]
    public class IsAllyTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller)
        {
            return owner.Faction == targeteable.Faction;
        }
    }
}
