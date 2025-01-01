using Game.UI.Windows;
using System.Collections;

namespace Game
{
    public partial class MainMenu : IGameState
    {
        public void Load()
        {
            GameSceneManager.Instance.StartCoroutine(Operation());

            IEnumerator Operation()
            {
                yield return GameSceneManager.Instance.Load(GameSceneManager.Instance.MainMenuSceneDefinition);

                LevelSelectionWindow levelSelectionWindow = WindowManager.Instance.GetWindow<LevelSelectionWindow>();
                levelSelectionWindow.Show();
            }
        }

        public void Update()
        {
        }

        public void Exit()
        {
            LevelSelectionWindow levelSelectionWindow = WindowManager.Instance.GetWindow<LevelSelectionWindow>();
            levelSelectionWindow.Hide();
        }
    }
}