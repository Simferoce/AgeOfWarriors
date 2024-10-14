using System;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaIsAlly : AbilityTargetCriteria
    {
        public override bool Execute(Ability source, Entity targetEntity)
        {
            return source.Faction == targetEntity.Faction;
        }
    }
}
