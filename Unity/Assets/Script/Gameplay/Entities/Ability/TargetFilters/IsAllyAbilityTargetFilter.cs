using Game.Agent;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsAllyAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return targetEntity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity targetIdentity)
                && source.Faction == targetIdentity.Faction;
        }
    }
}
