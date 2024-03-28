using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Factory
    {
        public class Command
        {
            public Action Action;
            public float ProductionDuration;
            public AgentObjectDefinition LaneObjectDefinition;
        }

        [SerializeField] private int commandSlot = 1;
        [SerializeField] private List<AgentObjectDefinition> agentObjectsDefinition = new List<AgentObjectDefinition>();

        public float TimeBeforeNextProductionNormalized => commands.Count == 0 ? -1 : (Time.time - productionStart) / commands[0].ProductionDuration;
        public int AmountOfAgentObjectAvailable => agentObjectsDefinition.Count;

        private List<Command> commands = new List<Command>();
        private int currentSpawnNumber;
        private float productionStart;
        private Agent agent;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public AgentObjectDefinition GetAgentObjectDefinitionAtIndex(int index)
        {
            return agentObjectsDefinition[index];
        }

        public bool QueueLaneObject(SpawnPoint spawnPoint, AgentObjectDefinition laneObjectDefinition)
        {
            if (commands.Count >= commandSlot)
                return false;

            if (commands.Count == 0)
                productionStart = Time.time;

            if (agent.Currency < laneObjectDefinition.Cost)
                return false;

            agent.Currency -= laneObjectDefinition.Cost;

            commands.Add(new Command()
            {
                Action = () => laneObjectDefinition.Spawn(agent, spawnPoint.transform.position, currentSpawnNumber++, spawnPoint.Direction),
                ProductionDuration = laneObjectDefinition.ProductionDuration,
                LaneObjectDefinition = laneObjectDefinition
            });

            return true;
        }

        public void SpawnAgentObject(AgentObject agentObject)
        {
            agentObject.Spawn(agent, currentSpawnNumber++, agent.Direction);
        }

        public void Update()
        {
            if (commands.Count > 0 && productionStart + commands[0].ProductionDuration < Time.time)
            {
                commands[0].Action.Invoke();
                commands.Remove(commands[0]);

                if (commands.Count > 0)
                    productionStart = Time.time;
            }
        }
    }
}
