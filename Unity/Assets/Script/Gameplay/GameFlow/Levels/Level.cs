﻿using Game.Agent;
using Game.UI.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Level : IGameState
    {
        private LevelDefinition levelDefinition;
        private AgentLoadout playerLoadout;

        public Level(LevelDefinition levelDefinition, AgentLoadout playerLoadout)
        {
            this.levelDefinition = levelDefinition;
            this.playerLoadout = playerLoadout;
        }

        public void Load()
        {
            GameSceneManager.Instance.StartCoroutine(Operation());

            IEnumerator Operation()
            {
                yield return GameSceneManager.Instance.Load(levelDefinition.SceneDefinition);
                List<BaseEntity> baseEntities = Entity.All.OfType<BaseEntity>().OrderBy(x => x.transform.position.x).ToList();

                GameObject agentGameObject = new GameObject($"Agent - {FactionType.Player}");
                AgentEntity playerAgent = agentGameObject.AddComponent<AgentEntity>();
                playerAgent.Initialize(new AgentBehaviourPlayer(), playerLoadout, FactionType.Player, baseEntities.FirstOrDefault(), 1);

                GameObject opponentGameObject = new GameObject($"Agent - {FactionType.Opponent}");
                AgentEntity opponentAgent = opponentGameObject.AddComponent<AgentEntity>();
                opponentAgent.Initialize(new AgentBehaviourAI(), levelDefinition.Loadout, FactionType.Opponent, baseEntities.LastOrDefault(), -1);

                HudWindow hudWindow = WindowManager.Instance.GetWindow<HudWindow>();
                hudWindow.Show();
            }
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}