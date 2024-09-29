using System;

namespace Game
{
    [Serializable]
    public class IsInjuredTargetCriteria : TargetCriteria
    {
        public override bool Execute(Entity source, Entity targetEntity)
        {
            return (targetEntity is Character character) && character.IsInjured;
        }
    }
}
