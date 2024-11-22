using UnityEngine;

namespace AgeOfWarriors.Unity
{
    [CreateAssetMenu(menuName = "Definition/CharacterDefinition", fileName = "CharacterDefinition")]
    public class CharacterDefinition : Definition, ICharacterDefinition
    {
        [SerializeField] private float speed;

        public float Speed => speed;
    }
}
