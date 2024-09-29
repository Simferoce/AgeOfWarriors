using System;

namespace Game
{
    [Serializable]
    public class IsAllyTargetCriteria : TargetCriteria
    {
        public override bool Execute(Entity source, Entity targetEntity)
        {
            return source.Faction == targetEntity.Faction;
        }
    }
}
