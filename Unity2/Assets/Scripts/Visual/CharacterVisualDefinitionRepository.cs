using AgeOfWarriors.Unity;
using AgeOfWarriors.Visual;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgeOfWarriors
{
    public class CharacterVisualDefinitionRepository : MonoBehaviour
    {
        [SerializeField] private List<CharacterVisualDefinition> characterVisualDefinitions;

        public CharacterVisualDefinition GetCorrespondingVisual(CharacterDefinition characterDefinition)
        {
            return characterVisualDefinitions.FirstOrDefault(x => x.CharacterDefinition == characterDefinition);
        }
    }
}
