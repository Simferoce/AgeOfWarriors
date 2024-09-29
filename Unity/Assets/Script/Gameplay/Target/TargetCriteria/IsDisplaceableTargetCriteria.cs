using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(Entity source, Entity targetEntity)
        {
            return (targetEntity is Character) && (targetEntity as AgentObject).IsActive;
        }
    }
}
