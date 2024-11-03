using Game.Agent;
using Game.Character;
using System;
using System.Linq;

namespace Game.Ability
{
    [Serializable]
    public class IsFirstAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            if (!ability.Caster.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity))
                return true;

            int minPriority = Entity.All.OfType<CharacterEntity>().Where(x => x.TryGetCachedComponent<AgentIdentity>(out AgentIdentity characterIdentity) && characterIdentity.Agent == agentIdentity.Agent && !x.IsDead).Min(x => x.GetCachedComponent<AgentIdentity>().Priority);
            return minPriority == agentIdentity.Priority;
        }
    }
}
