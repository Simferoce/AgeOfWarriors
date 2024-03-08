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
            public LaneObjectDefinition LaneObjectDefinition;
        }

        [SerializeField]
        private int commandSlot = 5;

        public List<Command> Commands { get => commands; }

        private List<Command> commands = new List<Command>();
        private int currentSpawnNumber;
        private float productionStart;

        public float TimeBeforeNextProductionNormalized => commands.Count == 0 ? -1 : (Time.time - productionStart) / commands[0].ProductionDuration;

        public void SpawnLaneObject(Agent agent, SpawnPoint spawnPoint, LaneObjectDefinition laneObjectDefinition)
        {
            if (commands.Count >= commandSlot)
                return;

            if (commands.Count == 0)
                productionStart = Time.time;

            commands.Add(new Command()
            {
                Action = () => laneObjectDefinition.Spawn(agent, currentSpawnNumber++, spawnPoint.Position, spawnPoint.Direction),
                ProductionDuration = laneObjectDefinition.ProductionDuration,
                LaneObjectDefinition = laneObjectDefinition
            });
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
