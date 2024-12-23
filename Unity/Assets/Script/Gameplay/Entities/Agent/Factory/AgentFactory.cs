﻿using Game.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Agent
{
    [Serializable]
    public class AgentFactory
    {
        [SerializeField] private int commandSlot = 1;

        public float TimeBeforeNextProductionNormalized => commands.Count == 0 ? -1 : 1 - commands[0].Progress;

        private List<AgentFactoryCommand> commands = new List<AgentFactoryCommand>();
        private AgentEntity agent;

        public void Initialize(AgentEntity agent)
        {
            this.agent = agent;
        }

        public bool QueueLaneObject(AgentFactoryCommand command, float cost)
        {
            if (commands.Count >= commandSlot)
                return false;

            if (Entity.All.OfType<CharacterEntity>().Count(x => x.GetCachedComponent<AgentIdentity>().Agent == agent) >= LevelSetup.Instance.MaxCharacter)
                return false;

            if (LevelSetup.Instance.CheatCost)
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
