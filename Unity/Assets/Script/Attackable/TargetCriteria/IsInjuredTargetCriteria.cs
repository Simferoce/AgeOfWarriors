using System;

namespace Game
{
    [Serializable]
    public class IsInjuredTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller)
        {
            return (targeteable is AgentObject agentObject) && agentObject.IsInjured;
        }
    }
}
