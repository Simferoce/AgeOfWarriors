using AgeOfWarriors.Core;
using AgeOfWarriors.Unity;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class VisualApplication : MonoBehaviour
    {
        [SerializeField] private CharacterVisualDefinitionRepository characterVisualDefinitionRepository;

        public void Initialize(GameApplication application)
        {
            application.Game.EventChannel.Subscribe<EntityCreatedEvent>(EntityCreated);
        }

        private void EntityCreated(EntityCreatedEvent evt)
        {
            if (evt.Entity is Character character && character.Definition is CharacterDefinition characterDefinition)
            {
                CharacterVisualDefinition characterVisualDefinition = characterVisualDefinitionRepository.GetCorrespondingVisual(characterDefinition);
                characterVisualDefinition.Instantiate(character);
            }
        }
    }
}
