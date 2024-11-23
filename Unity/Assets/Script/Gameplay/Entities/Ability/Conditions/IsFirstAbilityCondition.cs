using Game.Agent;
using Game.Character;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsFirstAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            if (!ability.Caster.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity))
                return true;

            int minPriority = int.MaxValue;
            foreach (var entity in Entity.All)
            {
                if (entity is CharacterEntity characterEntity
                    && characterEntity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity characterIdentity)
                    && characterIdentity.Agent == agentIdentity.Agent
                    && !characterEntity.IsDead)
                {
                    int priority = characterIdentity.Priority;
                    if (priority < minPriority)
                    {
                        minPriority = priority;
                    }
                }
            }

            return minPriority == agentIdentity.Priority;
        }
    }
}
