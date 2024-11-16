using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class CharacterVisual : MonoBehaviour
    {
        public Character Character { get => character; }

        private Character character;

        public void Initialize(Character character)
        {
            this.character = character;
        }
    }
}