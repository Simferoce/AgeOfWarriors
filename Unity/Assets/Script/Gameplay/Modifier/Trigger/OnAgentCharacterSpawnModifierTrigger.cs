using NUnit.Framework;
using System;

namespace Game
{
    [Serializable]
    public class OnAgentCharacterSpawnModifierTrigger : ModifierTrigger
    {
        public override void Initialize(Modifier modifier)
        {
            base.Initialize(modifier);

            Agent agent = modifier.Handler.Entity as Agent;
            Assert.IsNotNull(agent, $"Expecting the type of the entity of {nameof(ModifierHandler)} to be of type {nameof(Agent)}");

            agent.OnAgentObjectSpawn += AgentObjectSpawn;
        }

        private void AgentObjectSpawn(AgentObject agentObject)
        {
            Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();

            Agent agent = modifier.Handler.Entity as Agent;
            agent.OnAgentObjectSpawn -= AgentObjectSpawn;
        }
    }
}
