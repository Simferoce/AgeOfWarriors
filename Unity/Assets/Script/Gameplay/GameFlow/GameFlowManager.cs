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

        public void LoadMainMenu()
        {
            if (CurrentState != null)
                CurrentState.Exit();

            MainMenu mainMenu = new MainMenu();
            mainMenu.Load();

            CurrentState = mainMenu;
        }

        public void LoadLevel(LevelDefinition levelDefinition)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            Level level = new Level(levelDefinition);
            level.Load();

            CurrentState = level;
        }
    }
}
