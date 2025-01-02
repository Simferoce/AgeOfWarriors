using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class CommanderRepository : Manager<CommanderRepository>
    {
        private List<CommanderDefinition> commanderDefinitions = new List<CommanderDefinition>();
        private AsyncOperationHandle<IList<CommanderDefinition>> statisticsHandle;

        public override IEnumerator InitializeAsync()
        {
            statisticsHandle = Addressables.LoadAssetsAsync<CommanderDefinition>("Commander", (CommanderDefinition commanderDefinition) => commanderDefinitions.Add(commanderDefinition));
            yield return statisticsHandle;
        }

        public List<CommanderDefinition> GetAll()
        {
            return commanderDefinitions;
        }

        private void OnDestroy()
        {
            if (statisticsHandle.IsValid())
                Addressables.Release(statisticsHandle);
        }
    }
}
