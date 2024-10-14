using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]
    public class ModifierTriggerOnAgentCharacterSpawn : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeField] private AgentObjectDefinition affected;

        private List<object> targets = new List<object>();

        public override void Initialize(Modifier modifier)
        {
            base.Initialize(modifier);

            Agent agent = modifier.Handler.Entity as Agent;
            Assert.IsNotNull(agent, $"Expecting the type of the entity of {nameof(ModifierHandler)} to be of type {nameof(Agent)}");

            agent.OnAgentObjectSpawn += AgentObjectSpawn;
        }

        public bool IsValid(AgentObjectDefinition definition)
        {
            return definition == affected || definition.IsSpecialization(affected);
        }

        private void AgentObjectSpawn(AgentObject agentObject)
        {
            if (IsValid(agentObject.GetDefinition()))
            {
                targets.Clear();
                targets.Add(agentObject);
                Trigger();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            Agent agent = modifier.Handler.Entity as Agent;
            agent.OnAgentObjectSpawn -= AgentObjectSpawn;
        }

        public List<object> GetTargets()
        {
            return targets;
        }
    }
}
