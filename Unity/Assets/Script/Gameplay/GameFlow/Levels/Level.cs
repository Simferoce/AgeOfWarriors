using Game.Agent;
using UnityEngine;

namespace Game
{
    public partial class Level : IGameState
    {
        public float TimeElapsed { get; private set; }

        private LevelDefinition levelDefinition;
        private AgentLoadout playerLoadout;
        private StateMachine stateMachine;

        public Level(LevelDefinition levelDefinition, AgentLoadout playerLoadout)
        {
            this.levelDefinition = levelDefinition;
            this.playerLoadout = playerLoadout;
        }

        public void Load()
        {
            stateMachine = new StateMachine();
            stateMachine.Initialize(new InitializationState(this));
        }

        public void Abandon()
        {
            if (stateMachine.Current is not ExecutionState)
            {
                Debug.LogError($"Can only abandon a level in the \"{typeof(ExecutionState)}\" state. Currently in \"{stateMachine.Current.GetType()}\"");
                return;
            }

            stateMachine.ChangeState(new EndState(this, new AbandonEnd()));
        }

        public void Update()
        {
            stateMachine.Update();
        }

        public void Exit()
        {
            stateMachine.End();
        }
    }
}