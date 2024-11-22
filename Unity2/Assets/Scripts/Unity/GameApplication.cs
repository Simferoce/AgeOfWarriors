using AgeOfWarriors.Visual;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class GameApplication : MonoBehaviour
    {
        private Game game;
        private UnityDefinitionRepository definitionRepository;
        private VisualApplication visualApplication;

        private async void Start()
        {
            game = new Game();
            UnityDebug unityDebug = new UnityDebug();

            visualApplication = new VisualApplication();
            await visualApplication.Initialize(game);

            definitionRepository = new UnityDefinitionRepository(unityDebug);
            await definitionRepository.Initialize();

            game.Initialize(unityDebug, definitionRepository);
        }

        private void OnDestroy()
        {
            visualApplication.Dispose();
            definitionRepository.Dispose();
        }

        private void Update()
        {
            game.Update(UnityEngine.Time.deltaTime);
            visualApplication.Update();
        }

    }
}
