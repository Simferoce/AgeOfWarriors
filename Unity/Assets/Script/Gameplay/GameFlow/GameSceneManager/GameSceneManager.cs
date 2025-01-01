using System.Collections;
using UnityEngine;

namespace Game
{
    public class GameSceneManager : Manager<GameSceneManager>
    {
        [SerializeField] private SceneDefinition mainMenuSceneDefinition;

        public SceneDefinition MainMenuSceneDefinition { get => mainMenuSceneDefinition; set => mainMenuSceneDefinition = value; }

        private GameScene currentScene = null;

        public IEnumerator Load(SceneDefinition sceneDefinition)
        {
            if (currentScene != null)
                yield return currentScene.Unload();

            currentScene = new GameScene(sceneDefinition);
            yield return currentScene.Load();
        }

        public override IEnumerator InitializeAsync()
        {
            yield break;
        }
    }
}
