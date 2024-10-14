using System;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaIsDisplaceable : AbilityTargetCriteria
    {
        public override bool Execute(Ability source, Entity targetEntity)
        {
            return (targetEntity is Character) && (targetEntity as AgentObject).IsActive;
        }
    }
}
