using AgeOfWarriors.Core;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class UnityDefinitionRepository : MonoBehaviour, IDefinitionRepository
    {
        [SerializeField] private CharacterDefinition shieldbearerDefinition;

        public ICharacterDefinition ShieldbearerDefinition => shieldbearerDefinition;

        private Dictionary<string, IDefinition> definitions = new Dictionary<string, IDefinition>();
        private IGameDebug debug;

        public UnityDefinitionRepository(IGameDebug debug)
        {
            this.debug = debug;
        }

        public T Get<T>(string id)
            where T : IDefinition
        {
            if (!definitions.ContainsKey(id))
            {
                debug.LogError($"Could not find \"{id}\" in {this}.");
                return default;
            }

            if (definitions[id] is not T definition)
            {
                debug.LogError($"\"{definitions[id]}\" is of type \"{definitions[id].GetType()}\" but was expecting \"{nameof(T)}\".");
                return default;
            }

            return (T)definitions[id];
        }
    }
}
