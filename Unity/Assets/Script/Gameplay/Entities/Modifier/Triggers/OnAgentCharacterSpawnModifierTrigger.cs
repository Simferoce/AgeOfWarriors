using Game.Agent;
using Game.Character;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Modifier
{
    [Serializable]
    public class OnAgentCharacterSpawnModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeField] private CharacterDefinition affected;

        private List<object> targets = new List<object>();

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            Agent.AgentEntity agent = modifier.Target.Entity as Agent.AgentEntity;
            Assert.IsNotNull(agent, $"Expecting the type of the entity of {nameof(ModifierHandler)} to be of type {nameof(Agent)}");

            agent.OnAgentObjectSpawn += AgentObjectSpawn;
        }

        public bool IsValid(CharacterDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        private void AgentObjectSpawn(AgentIdentity agentIdentity)
        {
            if (agentIdentity.Entity is CharacterEntity character && IsValid(character.GetDefinition()))
            {
                targets.Clear();
                targets.Add(character);
                Trigger();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            Agent.AgentEntity agent = modifier.Target.Entity as Agent.AgentEntity;
            agent.OnAgentObjectSpawn -= AgentObjectSpawn;
        }

        public List<object> GetTargets()
        {
            return targets;
        }
    }
}
