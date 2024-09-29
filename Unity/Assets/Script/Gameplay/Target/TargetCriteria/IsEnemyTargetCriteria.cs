using System;

namespace Game
{
    [Serializable]
    public class IsEnemyTargetCriteria : TargetCriteria
    {
        public override bool Execute(Entity source, Entity targetEntity)
        {
            return source.Faction != targetEntity.Faction;
        }
    }
}
