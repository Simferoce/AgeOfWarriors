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
        [SerializeField] private CommanderDefinition commanderDefinition;

        public int AmountOfAgentObjectAvailable => commanderDefinition.CharacterDefinitions.Count;
        public CommanderDefinition CommanderDefinition { get => commanderDefinition; set => commanderDefinition = value; }
        public List<CharacterDefinition> CharacterDefinitions { get => commanderDefinition.CharacterDefinitions; }

        private AgentEntity agent;

        public void Initialize(AgentEntity agent)
        {
            this.agent = agent;
        }

        public CharacterDefinition GetCharacterDefinitionAtIndex(int index)
        {
            if (index >= commanderDefinition.CharacterDefinitions.Count)
                return null;

            CharacterDefinition agentIdentityDefinition = commanderDefinition.CharacterDefinitions[index];
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
