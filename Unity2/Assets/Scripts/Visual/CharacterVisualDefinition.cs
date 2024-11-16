using AgeOfWarriors.Unity;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    [CreateAssetMenu(menuName = "Definition/CharacterVisualDefinition", fileName = "CharacterVisualDefinition")]
    public class CharacterVisualDefinition : ScriptableObject
    {
        [SerializeField] private GameObject visual;
        [SerializeField] private CharacterDefinition characterDefinition;

        public CharacterDefinition CharacterDefinition { get => characterDefinition; set => characterDefinition = value; }

        public CharacterVisual Instantiate(Character character)
        {
            GameObject gameObjectCharacter = GameObject.Instantiate(visual);
            CharacterVisual characterVisual = gameObjectCharacter.GetComponent<CharacterVisual>();
            characterVisual.Initialize(character);

            return characterVisual;
        }
    }
}
