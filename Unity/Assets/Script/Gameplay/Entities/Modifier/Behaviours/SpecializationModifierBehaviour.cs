using Game.Character;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SpecializationModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private CharacterDefinition specializations;

        public CharacterDefinition Specialization { get => specializations; set => specializations = value; }
    }
}
