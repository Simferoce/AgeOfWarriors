using Game.Character;
using Game.Modifier;
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
            ModifierHandler modifierHandler = agent.GetCachedComponent<ModifierHandler>();
            foreach (ModifierEntity modifier in modifierHandler.GetModifiers())
            {
                foreach (ModifierBehaviour modifierBehaviour in modifier.Behaviours)
                {
                    if (modifierBehaviour is not SpecializationModifierBehaviour specializationModifierBehaviour)
                        continue;

                    if (specializationModifierBehaviour.Specialization.IsSpecialization(agentIdentityDefinition))
                        return specializationModifierBehaviour.Specialization;
                }
            }

            return agentIdentityDefinition;
        }
    }
}
