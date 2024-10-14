using System;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaIsInjured : AbilityTargetCriteria
    {
        public override bool Execute(Ability source, Entity targetEntity)
        {
            return (targetEntity is Character character) && character.IsInjured;
        }
    }
}
