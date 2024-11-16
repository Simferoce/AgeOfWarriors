using AgeOfWarriors.Visual;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class GameApplication : MonoBehaviour
    {
        [SerializeField] private UnityDefinitionRepository definitionRepository;
        [SerializeField] private VisualApplication visualApplication;

        public Game Game { get => game; }

        private Game game;

        private void Awake()
        {
            game = new Game();
            UnityDebug unityDebug = new UnityDebug();

            game.Initialize(unityDebug, definitionRepository);
            visualApplication.Initialize(this);
        }

        private void Update()
        {
            game.Update(UnityEngine.Time.deltaTime);
        }

    }
}
