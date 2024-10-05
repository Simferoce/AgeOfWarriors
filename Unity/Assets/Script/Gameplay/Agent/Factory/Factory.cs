using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Factory
    {
        [SerializeField] private int commandSlot = 1;

        public float TimeBeforeNextProductionNormalized => commands.Count == 0 ? -1 : 1 - commands[0].Progress;

        private List<FactoryCommand> commands = new List<FactoryCommand>();
        private Agent agent;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public bool QueueLaneObject(FactoryCommand command, float cost)
        {
            if (commands.Count >= commandSlot)
                return false;

            if (AgentObject.All.Where(x => x is Character).Count(x => x.Agent == agent) >= Level.Instance.MaxCharacter)
                return false;

            if (Level.Instance.CheatCost)
            {
                if (agent.Currency < 1)
                    return false;

                agent.Currency -= 1;
            }
            else
            {
                if (agent.Currency < cost)
                    return false;

                agent.Currency -= cost;
            }

            command.Start();

            commands.Add(command);
            return true;
        }

        public void Update()
        {
            for (int i = commandSlot - 1; i >= 0; --i)
            {
                if (commands.Count > i && commands[i].IsFinish())
                {
                    commands[i].Execute(this);
                    commands.RemoveAt(i);
                }
            }
        }
    }
}
