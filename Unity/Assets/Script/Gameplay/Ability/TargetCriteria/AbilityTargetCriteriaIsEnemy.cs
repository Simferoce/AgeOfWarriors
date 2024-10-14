using System;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaIsEnemy : AbilityTargetCriteria
    {
        public override bool Execute(Ability source, Entity targetEntity)
        {
            return source.Faction != targetEntity.Faction;
        }
    }
}
