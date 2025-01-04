using Game.Agent;
using Game.UI.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Level
    {
        public class InitializationState : State
        {
            public InitializationState(Level level) : base(level)
            {
            }

            public override void Enter()
            {
                GameSceneManager.Instance.StartCoroutine(Operation());

                IEnumerator Operation()
                {
                    yield return GameSceneManager.Instance.Load(level.levelDefinition.SceneDefinition);
                    List<BaseEntity> baseEntities = Entity.All.OfType<BaseEntity>().OrderBy(x => x.transform.position.x).ToList();

                    GameObject agentGameObject = new GameObject($"Agent - {FactionType.Player}");
                    AgentEntity playerAgent = agentGameObject.AddComponent<AgentEntity>();
                    playerAgent.Initialize(new AgentBehaviourPlayer(), level.playerLoadout, FactionType.Player, baseEntities.FirstOrDefault(), 1);

                    GameObject opponentGameObject = new GameObject($"Agent - {FactionType.Opponent}");
                    AgentEntity opponentAgent = opponentGameObject.AddComponent<AgentEntity>();
                    opponentAgent.Initialize(new AgentBehaviourAI(), level.levelDefinition.Loadout, FactionType.Opponent, baseEntities.LastOrDefault(), -1);

                    HudWindow hudWindow = WindowManager.Instance.GetWindow<HudWindow>();
                    hudWindow.Show();

                    level.stateMachine.ChangeState(new ExecutionState(level));
                }
            }

            public override void Exit()
            {
            }

            public override void Update()
            {
            }
        }
    }
}
