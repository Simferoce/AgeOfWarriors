using Game.Agent;
using System.Collections;

namespace Game
{
    public class GameFlowManager : Manager<GameFlowManager>
    {
        public IGameState CurrentState { get; private set; }

        public override IEnumerator InitializeAsync()
        {
            yield break;
        }

        private void Update()
        {
            if (CurrentState != null)
                CurrentState.Update();
        }

        public void LoadMainMenu()
        {
            if (CurrentState != null)
                CurrentState.Exit();

            MainMenu mainMenu = new MainMenu();
            mainMenu.Load();

            CurrentState = mainMenu;
        }

        public void LoadLevel(LevelDefinition levelDefinition, AgentLoadout playerLoadout)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            Level level = new Level(levelDefinition, playerLoadout);
            level.Load();

            CurrentState = level;
        }
    }
}
