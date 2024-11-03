using Game.Character;
using Game.Technology;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent
{
    [Serializable]
    public class AgentLoadout
    {
        [SerializeField] private List<CharacterDefinition> agentIdentitysDefinition = new List<CharacterDefinition>();

        public int AmountOfAgentObjectAvailable => agentIdentitysDefinition.Count;

        private AgentEntity agent;

        public void Initialize(AgentEntity agent)
        {
            this.agent = agent;
        }

        public CharacterDefinition GetCharacterDefinitionAtIndex(int index)
        {
            if (index >= agentIdentitysDefinition.Count)
                return null;

            CharacterDefinition agentIdentityDefinition = agentIdentitysDefinition[index];
            SpecializationTechnologyPerkDefinition specializationTechnologyPerkDefinition = agent.Technology.GetFirst(x => x is SpecializationTechnologyPerkDefinition s && s.Specialization.IsSpecialization(agentIdentityDefinition)) as SpecializationTechnologyPerkDefinition;
            if (specializationTechnologyPerkDefinition != null)
                return specializationTechnologyPerkDefinition.Specialization;

            return agentIdentityDefinition;
        }
    }
}
