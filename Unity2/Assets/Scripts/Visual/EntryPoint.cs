using AgeOfWarriors.Core;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private UnityDefinitionRepository definitionRepository;

        private Game game;

        private void Awake()
        {
            game = new Game();
            UnityDebug unityDebug = new UnityDebug();

            game.Initialize(unityDebug, definitionRepository);
            game.EventChannel.Subscribe<EntityCreatedEvent>(EntityCreated);
        }

        private void Update()
        {
            game.Update(UnityEngine.Time.deltaTime);
        }

        private void EntityCreated(EntityCreatedEvent evt)
        {
            if (evt.Entity is Character character && character.Definition is CharacterDefinition characterDefinition)
            {
                characterDefinition.Instantiate(character);
            }
        }
    }
}
