using AgeOfWarriors;
using AgeOfWarriors.Unity;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    [CreateAssetMenu(menuName = "Definition/CharacterVisualDefinition", fileName = "CharacterVisualDefinition")]
    public class CharacterVisualDefinition : VisualDefinition
    {
        [SerializeField] private GameObject visual;
        [SerializeField] private CharacterDefinition characterDefinition;

        public CharacterDefinition CharacterDefinition { get => characterDefinition; set => characterDefinition = value; }

        public override EntityVisual Instantiate(Entity entity)
        {
            GameObject gameObjectCharacter = GameObject.Instantiate(visual);
            CharacterVisual characterVisual = gameObjectCharacter.GetComponent<CharacterVisual>();
            characterVisual.Initialize(entity as Character);

            return characterVisual;
        }

        public override bool IsVisualFor(Entity entity)
        {
            return entity is Character character && character.Definition is CharacterDefinition characterDefinition && characterDefinition == this.characterDefinition;
        }
    }
}
