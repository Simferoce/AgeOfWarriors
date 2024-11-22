using AgeOfWarriors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgeOfWarriors.Unity
{
    public class UnityDefinitionRepository : IDefinitionRepository
    {
        private AddressablesExtensions.AddressablesHandle<IDefinition> handle;
        private Dictionary<string, IDefinition> definitions = new Dictionary<string, IDefinition>();
        private IGameDebug debug;

        public UnityDefinitionRepository(IGameDebug debug)
        {
            this.debug = debug;
        }

        public async Awaitable Initialize()
        {
            handle = await AddressablesExtensions.LoadAssetsAsync<IDefinition>("Definition");
            definitions = handle.Result.ToDictionary(x => x.Id);
        }

        public void Dispose()
        {
            handle.Dispose();
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
