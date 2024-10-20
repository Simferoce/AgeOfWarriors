using Game.Agent;
using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Ability
{
    [Serializable]
    public class PriorityAbilityTargetOrderBy : AbilityTargetOrderBy
    {
        public override IOrderedEnumerable<Target> OrderBy(IEnumerable<Target> targets)
        {
            if (targets is IOrderedEnumerable<Target> orderedTargets)
                return orderedTargets.ThenBy(x => x.Entity is AgentObject agentObject ? agentObject.Priority : -1);

            return targets.OrderBy(x => x.Entity is AgentObject agentObject ? agentObject.Priority : -1);
        }
    }
}
