using Game.Agent;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsEnemyAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return targetEntity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity targetIdentity)
                && source.Faction != targetIdentity.Faction;
        }
    }
}
