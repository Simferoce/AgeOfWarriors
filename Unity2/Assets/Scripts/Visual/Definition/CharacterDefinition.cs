using UnityEngine;

namespace AgeOfWarriors.Unity
{
    [CreateAssetMenu(menuName = "Definition/CharacterDefinition", fileName = "CharacterDefinition")]
    public class CharacterDefinition : Definition, ICharacterDefinition
    {
        [SerializeField] private GameObject visual;

        public CharacterVisual Instantiate(Character character)
        {
            GameObject gameObjectCharacter = GameObject.Instantiate(visual);
            CharacterVisual characterVisual = gameObjectCharacter.GetComponent<CharacterVisual>();
            characterVisual.Initialize(character);

            return characterVisual;
        }
    }
}
