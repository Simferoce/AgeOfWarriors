using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class CharacterVisual : MonoBehaviour
    {
        public Character Character { get => character; }

        private Character character;
        private CharacterAnimator characterAnimator;

        public void Initialize(Character character)
        {
            this.character = character;
            this.characterAnimator = GetComponentInChildren<CharacterAnimator>();
        }

        private void Update()
        {
            System.Numerics.Vector3 position = character.GetComponent<AgeOfWarriors.Core.Transform>().Position;
            this.transform.position = new Vector3(position.X, position.Y, position.Z);

            characterAnimator.SetFloat("SpeedRatio", character.CurrentSpeed / character.Speed);
        }
    }
}