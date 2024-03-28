using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Factory
    {
        public class Command
        {
            public Action Action;
            public float ProductionDuration;
            public AgentObjectDefinition LaneObjectDefinition;
        }

        [SerializeField]
        private int commandSlot = 1;

        public float TimeBeforeNextProductionNormalized => commands.Count == 0 ? -1 : (Time.time - productionStart) / commands[0].ProductionDuration;

        private List<Command> commands = new List<Command>();
        private int currentSpawnNumber;
        private float productionStart;
        private Agent agent;

        public Factory(Agent agent)
        {
            this.agent = agent;
        }

        public void QueueLaneObject(SpawnPoint spawnPoint, AgentObjectDefinition laneObjectDefinition)
        {
            if (commands.Count >= commandSlot)
                return;

            if (commands.Count == 0)
                productionStart = Time.time;

            if (!agent.InfiniteMoney && agent.Currency < laneObjectDefinition.Cost)
                return;

            agent.Currency -= laneObjectDefinition.Cost;

            commands.Add(new Command()
            {
                Action = () => laneObjectDefinition.Spawn(agent, spawnPoint.transform.position, currentSpawnNumber++, spawnPoint.Direction),
                ProductionDuration = laneObjectDefinition.ProductionDuration,
                LaneObjectDefinition = laneObjectDefinition
            });
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
