using Game.Technology;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent
{
    [Serializable]
    public class AgentLoadout
    {
        [SerializeField] private List<AgentObjectDefinition> agentObjectsDefinition = new List<AgentObjectDefinition>();

        public int AmountOfAgentObjectAvailable => agentObjectsDefinition.Count;

        private AgentEntity agent;

        public void Initialize(AgentEntity agent)
        {
            this.agent = agent;
        }

        public AgentObjectDefinition GetAgentObjectDefinitionAtIndex(int index)
        {
            if (index >= agentObjectsDefinition.Count)
                return null;

            AgentObjectDefinition agentObjectDefinition = agentObjectsDefinition[index];
            SpecializationTechnologyPerkDefinition specializationTechnologyPerkDefinition = agent.Technology.GetFirst(x => x is SpecializationTechnologyPerkDefinition s && s.Specialization.IsSpecialization(agentObjectDefinition)) as SpecializationTechnologyPerkDefinition;
            if (specializationTechnologyPerkDefinition != null)
                return specializationTechnologyPerkDefinition.Specialization;

            return agentObjectDefinition;
        }
    }
}
