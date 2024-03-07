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
            public float TimeToExecute;
            public LaneObjectDefinition LaneObjectDefinition;
        }

        private List<Command> commands = new List<Command>();

        private int currentSpawnNumber;
        private int commandSlot;

        public List<Command> Commands { get => commands; }

        public void SpawnLaneObject(Agent agent, SpawnPoint spawnPoint, LaneObjectDefinition laneObjectDefinition)
        {
            if (commands.Count >= commandSlot)
                return;

            commands.Add(new Command()
            {
                Action = () => laneObjectDefinition.Spawn(agent, currentSpawnNumber++, spawnPoint.Position, spawnPoint.Direction),
                TimeToExecute = Time.time + laneObjectDefinition.ProductionDuration,
                LaneObjectDefinition = laneObjectDefinition
            });
        }

        public void Update()
        {
            if (commands.Count > 0 && commands[0].TimeToExecute > Time.time)
            {
                commands[0].Action.Invoke();
                commands.Remove(commands[0]);
            }
        }
    }
}
